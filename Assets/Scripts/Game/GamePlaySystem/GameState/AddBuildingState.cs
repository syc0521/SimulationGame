using System.Runtime.CompilerServices;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.StateMachine;
using Game.GamePlaySystem.Task;
using Game.Input;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.GamePlaySystem.GameState
{
    public class AddBuildingState : StateBase
    {
        private int currentBuildingType;
        protected GameObject currentBuilding;
        private int rotation = 0;
        private uint currentID = 0;
        private Vector3 spawnPos;
        public override void OnEnter(params object[] list)
        {
            currentID = BuildingManager.Instance.GetID();
            currentBuildingType = (int)list[0];
            var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.safeArea.width / 2, Screen.safeArea.height / 2, 0));
            var point = ray.origin - ray.direction * (ray.origin.y / ray.direction.y);

            spawnPos = GetBlockPos(point, out _);
            currentBuilding = Object.Instantiate(ConfigTable.Instance.GetBuilding(currentBuildingType), spawnPos, Quaternion.identity);
            MaterialUtil.SetTransparency(currentBuilding, true);
            EventCenter.DispatchEvent(new BuildUIEvent());

            EventCenter.AddListener<TouchEvent>(PlaceBuilding);
            EventCenter.AddListener<RotateEvent>(RotateBuilding);
        }

        public override void OnUpdate()
        {
            UpdateScreenPos();
        }

        private void UpdateScreenPos()
        {
            if (currentBuilding == null || Camera.main == null) return;
            var screenPos = Camera.main.WorldToScreenPoint(spawnPos);
            BuildingManager.Instance.ScreenPos = new Vector3(screenPos[0] - 100, screenPos[1] - 100, 0);
        }

        public override void OnLeave(params object[] list)
        {
            EventCenter.RemoveListener<RotateEvent>(RotateBuilding);
            if ((bool)list[0]) //可以建造
            {
                ConstructBuilding();
            }
            Object.Destroy(currentBuilding);
            currentBuilding = null;
        }

        /// <summary>
        /// 摆放建筑物，点击位置后移动，需要通知UI位置
        /// 摆放建筑物阶段用传统模式（保证效果），添加后用ECS（保证效率）
        /// </summary>
        private void PlaceBuilding(TouchEvent evt)
        {
            if (currentBuilding == null) return;

            var collisionWorld = BuildingManager.Instance.GetCollisionWorld();
            var raycastInput = BuildingManager.Instance.GetRaycastInput(new float3(evt.pos, 0));

            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entity = hit.Entity;
                if (entityManager.HasComponent<BuildingPlane>(entity) && !Managers.Get<IInputManager>().IsPointerOverGameObject())
                {
                    spawnPos = GetBlockPos(hit.Position, out _);
                    var data = ConfigTable.Instance.GetBuildingData(currentBuildingType);
                    var offset = BuildingManager.Instance.GetRotationOffset(rotation, data.Rowcount, data.Colcount);
                    currentBuilding.transform.position = spawnPos + (Vector3)offset;
                    //todo 增加建筑遮挡判定
                    EventCenter.DispatchEvent(new BuildUIEvent());
                }
            }
        }

        /// <summary>
        /// ECS构建建筑物
        /// </summary>
        private void ConstructBuilding()
        {
            var data = ConfigTable.Instance.GetBuildingData(currentBuildingType);
            var offset = BuildingManager.Instance.GetRotationOffset(rotation, data.Rowcount, data.Colcount);

            var buildingPos = currentBuilding.transform.position;
            var pos = buildingPos - (Vector3)offset;
            BuildingManager.Instance.SetBuildingData(currentID, new BuildingData
            {
                level = 1,
                position = ((float3)pos).xz,
                rotation = rotation,
                type = currentBuildingType
            });
            
            var blockPos = GetBlockPos(pos, out _);
            var grid = BuildingManager.Instance.GetGrid();

            GetBlockPos(buildingPos, out var gridPos);
            var row = rotation % 2 == 0 ? data.Rowcount : data.Colcount;
            var col = rotation % 2 == 0 ? data.Colcount : data.Rowcount;
            for (int i = gridPos[0]; i < gridPos[0] + row; i++)
            {
                for (int j = gridPos[1]; j < gridPos[1] + col; j++)
                {
                    grid[i, j] = currentBuildingType;
                }
            }

            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AddBlockSystem>().Build(blockPos, currentBuildingType, currentID, rotation);
            EventCenter.RemoveListener<TouchEvent>(PlaceBuilding);
            TaskManager.Instance.TriggerTask(TaskType.AddBuilding, currentBuildingType);
        }
        
        private void RotateBuilding(RotateEvent evt)
        {
            rotation = (rotation + 1) % 4;
            var objTransform = currentBuilding.transform;
            objTransform.Rotate(Vector3.up, 90);
            var data = ConfigTable.Instance.GetBuildingData(currentBuildingType);
            var offset = BuildingManager.Instance.GetRotationOffset(rotation, data.Rowcount, data.Colcount);
            objTransform.position = spawnPos + (Vector3)offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected float3 GetBlockPos(float3 pos, out int2 gridPos)
        {
            var xzpos = math.floor(pos.xz);
            gridPos = math.int2(xzpos);
            return new float3(xzpos[0], pos.y, xzpos[1]);
        }
    }
}