﻿using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.BurstUtil;
using Game.GamePlaySystem.GameState;
using Game.GamePlaySystem.StateMachine;
using Game.Input;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using Grid = Game.Data.Grid;

namespace Game.GamePlaySystem
{
    public class BuildingManager : GamePlaySystemBase<BuildingManager>
    {
        private Dictionary<uint, BuildingData> _buildingDatas;
        private Grid grid;
        private StateMachine.StateMachine buildStateMachine;
        private uint id;
        private int col, row;
        public Vector3 ScreenPos { get; set; }

        public override void OnStart()
        {
            col = ConfigTable.Instance.GetBuildConfig().col;
            row = ConfigTable.Instance.GetBuildConfig().row;
            grid = new(col, row, -1);
            Managers.Get<ISaveDataManager>().GetBuildings(ref _buildingDatas);
            InitializeBuilding();
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

        public override void OnDestroyed()
        {
            buildStateMachine.ChangeState<NormalState>(false);
            buildStateMachine.Dispose();
            EventCenter.RemoveListener<LongPressEvent>(SelectBuilding);
            base.OnDestroyed();
        }

        private void InitializeBuilding()
        {
            foreach (var (key, data) in _buildingDatas)
            {
                if (id < key)
                {
                    id = key;
                }
                
                var pos = new float3(data.position[0], 0f, data.position[1]);
                Build(new float3(data.position[0], 0f, data.position[1]), data.type, key, data.rotation);
                SetGridData(pos, data.rotation, data.type, data.type);
            }
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
            EventCenter.DispatchEvent(new RotateEvent());
        }

        public void TransitToNormalState()
        {
            buildStateMachine.ChangeState<NormalState>();
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

        private bool DetectBuilding(float2 pos, out Entity entity)
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

        public uint GetID()
        {
            return ++id;
        }

        public void SetBuildingData(uint buildingId, BuildingData data)
        {
            _buildingDatas[buildingId] = data;
            Managers.Get<ISaveDataManager>().SaveData();
        }

        public void RemoveBuildingData(uint buildingId)
        {
            _buildingDatas.Remove(buildingId);
        }

        public BuildingData GetBuildingData(uint buildingId) => _buildingDatas[buildingId];

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
        
        public void Build(float3 position, int buildingType, uint id, int rotation = 0)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var newBlock = entityManager.CreateEntity();
            var data = ConfigTable.Instance.GetBuildingData(buildingType);
            var offset = GetRotationOffset(rotation, data.Rowcount, data.Colcount);
            entityManager.AddComponentData(newBlock, new AddBuilding
            {
                id = id,
                spawnPos = position,
                spawnType = buildingType,
                rotation = rotation,
                offset = offset,
            });
        }

    }
}