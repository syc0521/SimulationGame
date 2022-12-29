using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.GamePlaySystem.GameState;
using Game.GamePlaySystem.StateMachine;
using Game.Input;
using Game.LevelAndEntity.Component;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Game.GamePlaySystem
{
    public class BuildingManager : GamePlaySystemBase<BuildingManager>
    {
        public Camera mainCamera;

        private Dictionary<uint, BuildingData> _buildingDatas;
        private RaycastInput _raycastInput;
        private bool hasRaycastInput;
        private Grid<int> grid = new(15, 15, -1);
        private StateMachine.StateMachine buildStateMachine;
        private uint id;

        public override void OnStart()
        {
            _buildingDatas = Managers.Get<ISaveDataManager>().GetBuildings();
            buildStateMachine = new(new IState[]
            {
                new NormalState(),
                new ModifyState(),
                new AddBuildingState(),
                new DestroyState(),
            });
            buildStateMachine.ChangeState<NormalState>();
            EventCenter.AddListener<LongPressEvent>(SelectBuilding);
        }
        
        private void SelectBuilding(LongPressEvent evt)
        {
            var entity = Entity.Null;
            Debug.Log(buildStateMachine.GetCurrentState());
            if (buildStateMachine.GetCurrentState() == "NormalState" && DetectBuilding(evt.pos, out entity))
            {
                buildStateMachine.ChangeState<ModifyState>(entity);
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
            
        }

        public void TransitToNormalState()
        {
            buildStateMachine.ChangeState<NormalState>();
        }
        
        public RaycastInput GetOrCreateRaycastInput(float3 pos)
        {
            if (hasRaycastInput)
            {
                return _raycastInput;
            }
            var ray = mainCamera.ScreenPointToRay(pos);
            _raycastInput = new RaycastInput
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
            return _raycastInput;
        }

        public CollisionWorld GetCollisionWorld()
        {
            var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();
            var singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
            var collisionWorld = singletonQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            singletonQuery.Dispose();
            return collisionWorld;
        }

        public Grid<int> GetGrid() => grid;
        
        private bool DetectBuilding(float2 pos, out Entity entity)
        {
            entity = default;
            var collisionWorld = GetCollisionWorld();
            var raycastInput = GetOrCreateRaycastInput(new float3(pos, 0));
            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                entity = hit.Entity;
                return hit.Entity != Entity.Null && entityManager.HasComponent<Building>(entity);
            }

            return false;
        }

        public uint GetID()
        {
            return ++id;
        }

        public void SetBuildingData(uint buildingId, BuildingData data)
        {
            _buildingDatas[buildingId] = data;
            Managers.Get<ISaveDataManager>().SaveData();
        }

        public BuildingData GetBuildingData(uint buildingId) => _buildingDatas[buildingId];
    }
}