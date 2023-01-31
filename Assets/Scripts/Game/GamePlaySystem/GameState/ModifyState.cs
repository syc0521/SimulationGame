using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.BurstUtil;
using Game.Input;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game.GamePlaySystem.GameState
{
    public class ModifyState : AddBuildingState
    {
        private uint _currentId;
        private BuildingData _buildingUserData;
        private int currentBuildingType;
        private Entity buildingEntity;
        private float3 originPos;
        private int currentRotation = 0;
        private float3 spawnPos;
        
        public override void OnEnter(params object[] list)
        {
            buildingEntity = (Entity)list[0];
            SelectBuilding(buildingEntity);

            EventCenter.AddListener<TouchEvent>(PlaceBuilding);
            EventCenter.AddListener<RotateEvent>(RotateBuilding);
        }

        public override void OnLeave(params object[] list)
        {
            EventCenter.RemoveListener<TouchEvent>(PlaceBuilding);
            EventCenter.RemoveListener<RotateEvent>(RotateBuilding);

            var transform = World.DefaultGameObjectInjectionWorld.EntityManager.GetAspect<TransformAspect>(buildingEntity);
            if ((bool)list[0])
            {
                transform.Position = currentBuilding.transform.position;
                _buildingUserData.position = transform.Position.xz;
                _buildingUserData.rotation = currentRotation;
                BuildingManager.Instance.SetBuildingData(_currentId, _buildingUserData);
                
                transform.LocalRotation = quaternion.identity;
                transform.LocalRotation = quaternion.RotateY(math.radians(90 * currentRotation));
            }
            else
            {
                transform.Position = originPos;
            }

            float3 pos = (bool)list[0] ? currentBuilding.transform.position : originPos;
            BuildingManager.Instance.SetGridData(pos, currentRotation, currentBuildingType, currentBuildingType);

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
            
            var transform = entityManager.GetAspect<TransformAspect>(entity);
            var buildingPos = transform.Position;
            BuildingManager.Instance.SetGridData(buildingPos, currentRotation, currentBuildingType);

            originPos = buildingPos;
            
            var buildingRot = transform.Rotation;
            currentRotation = _buildingUserData.rotation;

            currentBuilding = Object.Instantiate(ConfigTable.Instance.GetBuilding(currentBuildingType), buildingPos, Quaternion.identity);
            currentBuilding.transform.localRotation = buildingRot;
            MaterialUtil.SetTransparency(currentBuilding, true);
            transform.Position = new float3(10000, 10000, 10000);
            EventCenter.DispatchEvent(new BuildUIEvent());
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
                if (entityManager.HasComponent<BuildingPlane>(entity) && !Managers.Get<IInputManager>().IsPointerOverGameObject())
                {
                    spawnPos = BuildingUtils.GetBlockPos(hit.Position);
                    var data = ConfigTable.Instance.GetBuildingData(currentBuildingType);
                    var offset = BuildingManager.Instance.GetRotationOffset(currentRotation, data.Rowcount, data.Colcount);
                    currentBuilding.transform.position = spawnPos + offset;
                    EventCenter.DispatchEvent(new BuildUIEvent());
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
        }
    }
}