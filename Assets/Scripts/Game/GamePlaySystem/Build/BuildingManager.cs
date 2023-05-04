using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.BurstUtil;
using Game.GamePlaySystem.GameState;
using Game.GamePlaySystem.StateMachine;
using Game.GamePlaySystem.Task;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using Grid = Game.Data.Grid;

namespace Game.GamePlaySystem.Build
{
    public partial class BuildingManager : GamePlaySystemBase<BuildingManager>
    {
        private Dictionary<uint, BuildingData> _buildingDatas;
        private Grid grid;
        private StateMachine.StateMachine buildStateMachine;
        private uint _internalID;
        private int col, row;
        private HashSet<int> unlockedBuildings;
        public Vector3 ScreenPos { get; set; }

        public override void OnAwake()
        {
            base.OnAwake();
            EventCenter.AddListener<LoadDataEvent>(GetUnlockedBuildings);
            col = ConfigTable.Instance.GetBuildConfig().col;
            row = ConfigTable.Instance.GetBuildConfig().row;
            grid = new(col, row, -1);
        }

        public override void OnStart()
        {
            buildStateMachine = new(new IState[]
            {
                new NormalState(),
                new ModifyState(),
                new AddBuildingState(),
                new DestroyState(),
                new AddRoadState(),
            });
            buildStateMachine.ChangeState<NormalState>();
            EventCenter.AddListener<StaticBuildingIntlEvent>(InitializeStaticBuilding);
        }

        public override void OnDestroyed()
        {
            buildStateMachine.ChangeState<NormalState>(false);
            buildStateMachine.Dispose();
            EventCenter.RemoveListener<LoadDataEvent>(GetUnlockedBuildings);
            EventCenter.RemoveListener<StaticBuildingIntlEvent>(InitializeStaticBuilding);
            
            grid.Dispose();
            base.OnDestroyed();
        }

        public void LoadBuildings()
        {
            Managers.Get<ISaveDataManager>().GetBuildings(ref _buildingDatas);
            InitializeBuilding();
        }

        private void GetUnlockedBuildings(LoadDataEvent evt)
        {
            Managers.Get<ISaveDataManager>().GetUnlockedBuildings(ref unlockedBuildings);
        }

        private void InitializeBuilding()
        {
            // 静态建筑只存数据，不生成entity
            foreach (var (key, data) in _buildingDatas.Where(pair => pair.Key < 10000))
            {
                if (_internalID < key)
                {
                    _internalID = key;
                }
                
                var pos = new float3(data.position[0], 0f, data.position[1]);
                Build(new float3(data.position[0], 0f, data.position[1]), data.type, key, data.rotation);
                SetGridData(pos, data.rotation, data.type, data.type);
            }
        }

        public void AddBuilding(int type)
        {
            buildStateMachine.ChangeState<AddBuildingState>(type);
        }

        public void ConstructBuilding()
        {
            buildStateMachine.ChangeState<NormalState>(true);
        }

        public void DeleteTempBuilding()
        {
            buildStateMachine.ChangeState<NormalState>(false);
        }

        public void RemoveBuilding()
        {
            buildStateMachine.ChangeState<DestroyState>();
        }

        public void RotateBuilding()
        {
            EventCenter.DispatchEvent(new RotateEvent());
        }

        public void TransitToNormalState()
        {
            buildStateMachine.ChangeState<NormalState>();
        }

        public void ModifyBuilding(Entity entity)
        {
            buildStateMachine.ChangeState<ModifyState>(entity);
        }

        public RaycastInput GetRaycastInput(float3 pos)
        {
            var ray = CameraManager.Instance.mainCam.ScreenPointToRay(pos);
            return new RaycastInput
            {
                Start = ray.origin,
                End = ray.GetPoint(50),
                Filter = new CollisionFilter
                {
                    BelongsTo = ~0u,
                    CollidesWith = ~0u, // all 1s, so all layers, collide with everything
                    GroupIndex = 0
                }
            };
        }

        public CollisionWorld GetCollisionWorld()
        {
            var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();
            var singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
            var collisionWorld = singletonQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            singletonQuery.Dispose();
            return collisionWorld;
        }

        public bool DetectDynamicBuilding(float2 pos, out Entity entity)
        {
            entity = default;
            var collisionWorld = GetCollisionWorld();
            var raycastInput = GetRaycastInput(new float3(pos, 0));
            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                entity = hit.Entity;
                return hit.Entity != Entity.Null && entityManager.HasComponent<Building>(entity);
            }

            return false;
        }
        
        public bool DetectBuilding(float2 pos, out Entity entity)
        {
            entity = default;
            var collisionWorld = GetCollisionWorld();
            var raycastInput = GetRaycastInput(new float3(pos, 0));
            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                entity = hit.Entity;
                return hit.Entity != Entity.Null && (entityManager.HasComponent<Building>(entity) || entityManager.HasComponent<StaticBuilding>(entity));
            }

            return false;
        }

        public uint GetID() => ++_internalID;

        public void SetBuildingData(uint buildingId, BuildingData data)
        {
            _buildingDatas[buildingId] = data;
            //Managers.Get<ISaveDataManager>().SaveData();
        }

        public void RemoveBuildingData(uint buildingId)
        {
            _buildingDatas.Remove(buildingId);
        }

        public BuildingData GetBuildingData(uint buildingId) => _buildingDatas[buildingId];

        public Dictionary<uint, BuildingData> GetAllBuildingData() => _buildingDatas;

        public float3 GetRotationOffset(int dir, int width, int height)
        {
            return dir switch
            {
                0 => float3.zero,
                1 => new float3(0, 0, width),
                2 => new float3(width, 0, height),
                3 => new float3(height, 0, 0),
                _ => float3.zero
            };
        }

        private void InitializeStaticBuilding(StaticBuildingIntlEvent evt)
        {
            if (!_buildingDatas.ContainsKey((uint)evt.id + 10000))
            {
                _buildingDatas[(uint)evt.id + 10000] = new BuildingData
                {
                    type = evt.id,
                    level = 1,
                };
            }
            BuildingUtils.SetGridData(ref grid, evt.pos, evt.row, evt.col, evt.id);
        }
        
        public void SetGridData(float3 pos, int rotation, int type, int value = -1)
        {
            var data = ConfigTable.Instance.GetBuildingData(type);
            var actualRow = rotation % 2 == 0 ? data.Rowcount : data.Colcount;
            var actualCol = rotation % 2 == 0 ? data.Colcount : data.Rowcount;
            BuildingUtils.SetGridData(ref grid, pos, actualRow, actualCol, value);
        }

        public bool CanConstruct(float3 pos, int rotation, int type)
        {
            var data = ConfigTable.Instance.GetBuildingData(type);
            var actualRow = rotation % 2 == 0 ? data.Rowcount : data.Colcount;
            var actualCol = rotation % 2 == 0 ? data.Colcount : data.Rowcount;
            return !BuildingUtils.HasBuilding(ref grid, pos, actualRow, actualCol);
        }
        
        public void Build(float3 position, int buildingType, uint buildingId, int rotation = 0)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var newBlock = entityManager.CreateEntity();
            var data = ConfigTable.Instance.GetBuildingData(buildingType);
            var offset = GetRotationOffset(rotation, data.Rowcount, data.Colcount);
            entityManager.AddComponentData(newBlock, new AddBuilding
            {
                id = buildingId,
                spawnPos = position,
                spawnType = buildingType,
                rotation = rotation,
                offset = offset,
            });
        }

        public bool UnlockBuilding(int buildingId)
        {
            if (unlockedBuildings.Contains(buildingId))
            {
                return false;
            }

            unlockedBuildings.Add(buildingId);
            return true;
        }

        public bool CheckBuildingUnlocked(int buildingId)
        {
            return ConfigTable.Instance.GetBuildingData(buildingId).Unlock || unlockedBuildings.Contains(buildingId);
        }

        public void UpgradeBuilding(uint buildingId, int newLevel, bool isStatic = false)
        {
            var system = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildingManagedSystem>();
            system.UpgradeBuilding(buildingId, newLevel, isStatic);
            _buildingDatas[buildingId].level = newLevel;
            var staticId = _buildingDatas[buildingId].type;
            TaskManager.Instance.TriggerTask(TaskType.UpgradeBuilding, staticId);
            EventCenter.DispatchEvent(new OpenBuildingInfoEvent
            {
                id = (int)buildingId,
                isStatic = isStatic,
            });
            Managers.Get<ISaveDataManager>().SaveData();
        }

        public bool TryGetStaticBuildingLevel(uint buildingId, out int level)
        {
            if (_buildingDatas.TryGetValue(buildingId, out var data))
            {
                level = data.level;
                return true;
            }

            level = 0;
            return false;
        }

        public int CountBuildingType(int type)
        {
            return _buildingDatas.Values.Count(data => ConfigTable.Instance.GetBuildingData(data.type).Buildingtype == type);
        }
    }
}