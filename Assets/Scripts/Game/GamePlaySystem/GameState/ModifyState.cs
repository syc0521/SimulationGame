using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.BurstUtil;
using Game.GamePlaySystem.StateMachine;
using Game.GamePlaySystem.Task;
using Game.Input;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game.GamePlaySystem.GameState
{
    public class ModifyState : StateBase
    {
        private uint _currentId;
        private BuildingData _buildingUserData;
        private GameObject currentBuilding;
        private int currentBuildingType;
        private Entity buildingEntity;
        private float3 originPos;
        private int currentRotation = 0;
        private float3 spawnPos;
        
        // 任务系统用
        private float3 previousPos;
        private int previousRot;
        
        public override void OnEnter(params object[] list)
        {
            buildingEntity = (Entity)list[0];
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AddBlockSystem>().SetGridVisible(true);
            SelectBuilding(buildingEntity);

            EventCenter.AddListener<TouchEvent>(PlaceBuilding);
            EventCenter.AddListener<RotateEvent>(RotateBuilding);
        }

        public override void OnLeave(params object[] list)
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AddBlockSystem>().SetGridVisible(false);
            EventCenter.RemoveListener<TouchEvent>(PlaceBuilding);
            EventCenter.RemoveListener<RotateEvent>(RotateBuilding);

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var aspect = entityManager.GetAspect<BuildingAspect>(buildingEntity);
            
            var data = ConfigTable.Instance.GetBuildingData(currentBuildingType);
            var offset = BuildingManager.Instance.GetRotationOffset(currentRotation, data.Rowcount, data.Colcount);
            
            if ((bool)list[0])
            {
                aspect.Position = currentBuilding.transform.position;
                _buildingUserData.position = (aspect.Position - offset).xz;
                _buildingUserData.rotation = currentRotation;
                aspect.SpawnPos = aspect.Position - offset;
                if (!previousPos.Equals(aspect.SpawnPos))
                {
                    TaskManager.Instance.TriggerTask(TaskType.MoveBuilding, aspect.BuildingType);
                }

                if (previousRot != currentRotation)
                {
                    TaskManager.Instance.TriggerTask(TaskType.RotateBuilding, aspect.BuildingType);
                }
                BuildingManager.Instance.SetBuildingData(_currentId, _buildingUserData);
                
                aspect.LocalRotation = quaternion.identity;
                aspect.LocalRotation = quaternion.RotateY(math.radians(90 * currentRotation));
            }
            else
            {
                aspect.Position = originPos;
            }

            // todo ecs层spawnPos更新
            BuildingManager.Instance.SetGridData(aspect.SpawnPos, currentRotation, currentBuildingType, currentBuildingType);

            Object.Destroy(currentBuilding);
            currentBuilding = null;
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

        /// <summary>
        /// 选中某个建筑物后的操作，需要给UI层发事件
        /// 选中后，ecs将模型移动到很远的地方（相当于不渲染模型），原地生成一个透明的GameObject
        /// 选中后的操作和摆放类似
        /// </summary>
        private void SelectBuilding(Entity entity)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var aspect = entityManager.GetAspect<BuildingAspect>(entity); // 获取ECS建筑信息
            _currentId = aspect.ID;
            _buildingUserData = BuildingManager.Instance.GetBuildingData(_currentId);
            currentBuildingType = aspect.BuildingType;
            spawnPos = aspect.SpawnPos;
            previousPos = aspect.SpawnPos;
            
            var buildingPos = aspect.Position;
            BuildingManager.Instance.SetGridData(spawnPos, currentRotation, currentBuildingType);

            originPos = buildingPos;
            currentRotation = _buildingUserData.rotation;
            previousRot = _buildingUserData.rotation;

            currentBuilding = Object.Instantiate(ConfigTable.Instance.GetBuilding(currentBuildingType), buildingPos, Quaternion.identity);
            currentBuilding.transform.localRotation = aspect.LocalRotation;
            MaterialUtil.SetTransparency(currentBuilding, true);
            aspect.Position = new float3(10000, 10000, 10000);
            EventCenter.DispatchEvent(new BuildUIEvent
            {
                canConstruct = BuildingManager.Instance.CanConstruct(spawnPos, currentRotation, currentBuildingType),
            });
        }
        
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
                    var offset = BuildingManager.Instance.GetRotationOffset(currentRotation, data.Rowcount, data.Colcount);
                    currentBuilding.transform.position = spawnPos + offset;
                    EventCenter.DispatchEvent(new BuildUIEvent
                    {
                        canConstruct = BuildingManager.Instance.CanConstruct(spawnPos, currentRotation, currentBuildingType),
                    });
                }
            }
        }

        private void RotateBuilding(RotateEvent evt)
        {
            currentRotation = (currentRotation + 1) % 4;
            var objTransform = currentBuilding.transform;
            objTransform.Rotate(Vector3.up, 90);
            var data = ConfigTable.Instance.GetBuildingData(currentBuildingType);
            var offset = BuildingManager.Instance.GetRotationOffset(currentRotation, data.Rowcount, data.Colcount);
            objTransform.position = spawnPos + offset;
            EventCenter.DispatchEvent(new BuildUIEvent
            {
                canConstruct = BuildingManager.Instance.CanConstruct(spawnPos, currentRotation, currentBuildingType),
            });
        }
    }
}