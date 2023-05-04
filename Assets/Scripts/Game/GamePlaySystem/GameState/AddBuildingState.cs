using Game.Core;
using Game.Data;
using Game.Data.Achievement;
using Game.Data.Event;
using Game.GamePlaySystem.Achievement;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.BurstUtil;
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
        private GameObject currentBuilding;
        private int rotation = 0;
        private uint currentID = 0;
        private Vector3 spawnPos;
        public override void OnEnter(params object[] list)
        {
            currentID = BuildingManager.Instance.GetID();
            currentBuildingType = (int)list[0];
            rotation = 0;
            var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.safeArea.width / 2, Screen.safeArea.height / 2, 0));
            var point = ray.origin - ray.direction * (ray.origin.y / ray.direction.y);

            spawnPos = BuildingUtils.GetBlockPos(point);
            currentBuilding = Object.Instantiate(ConfigTable.Instance.GetBuilding(currentBuildingType), spawnPos, Quaternion.identity);
            MaterialUtil.SetTransparency(currentBuilding);
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildingManagedSystem>().SetGridVisible(true);
            EventCenter.DispatchEvent(new BuildUIEvent
            {
                canConstruct = BuildingManager.Instance.CanConstruct(spawnPos, rotation, currentBuildingType),
            });

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
            
            var data = ConfigTable.Instance.GetBuildingData(currentBuildingType);
            var buildingPos = spawnPos + new Vector3(data.Rowcount / 2.0f, 0, data.Colcount / 2.0f);
            
            var screenPos = Camera.main.WorldToScreenPoint(buildingPos);
            BuildingManager.Instance.ScreenPos = new Vector3(screenPos[0] - 230, screenPos[1] - 150, 0);
        }

        public override void OnLeave(params object[] list)
        {
            var world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
            {
                return;
            }
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildingManagedSystem>().SetGridVisible(false);
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
                if (entityManager.HasComponent<BuildingPlane>(entity))
                {
                    spawnPos = BuildingUtils.GetBlockPos(hit.Position);
                    var data = ConfigTable.Instance.GetBuildingData(currentBuildingType);
                    var offset = BuildingManager.Instance.GetRotationOffset(rotation, data.Rowcount, data.Colcount);
                    var buildingPos = spawnPos + (Vector3)offset;
                    currentBuilding.transform.position = buildingPos;
                    EventCenter.DispatchEvent(new BuildUIEvent
                    {
                        canConstruct = BuildingManager.Instance.CanConstruct(spawnPos, rotation, currentBuildingType),
                    });
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
            
            var blockPos = BuildingUtils.GetBlockPos(pos);
            BuildingManager.Instance.SetGridData(buildingPos, rotation, currentBuildingType, currentBuildingType);

            BuildingManager.Instance.Build(blockPos, currentBuildingType, currentID, rotation);
            EventCenter.RemoveListener<TouchEvent>(PlaceBuilding);
            TaskManager.Instance.TriggerTask(TaskType.AddBuilding, currentBuildingType);
            TaskManager.Instance.TriggerTask(TaskType.CountBuilding, data.Buildingtype, 
                BuildingManager.Instance.CountBuildingType(data.Buildingtype));
            AchievementManager.Instance.TriggerAchievement(AchievementType.Building, -1, 1);
            AchievementManager.Instance.TriggerAchievement(AchievementType.BuildingCategory, data.Buildingtype, 1);
            AchievementManager.Instance.TriggerAchievement(AchievementType.BuildingID, currentBuildingType, 1);
        }
        
        private void RotateBuilding(RotateEvent evt)
        {
            rotation = (rotation + 1) % 4;
            var objTransform = currentBuilding.transform;
            objTransform.Rotate(Vector3.up, 90);
            var data = ConfigTable.Instance.GetBuildingData(currentBuildingType);
            var offset = BuildingManager.Instance.GetRotationOffset(rotation, data.Rowcount, data.Colcount);
            objTransform.position = spawnPos + (Vector3)offset;
            EventCenter.DispatchEvent(new BuildUIEvent
            {
                canConstruct = BuildingManager.Instance.CanConstruct(spawnPos, rotation, currentBuildingType),
            });
        }

    }
}