using System.Runtime.CompilerServices;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.StateMachine;
using Game.Input;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game.GamePlaySystem.GameState
{
    public class ModifyState : StateBase
    {
        private uint _currentId;
        private BuildingData _buildingData;
        private int currentBuildingType;
        private GameObject currentBuilding;
        private Entity buildingEntity;
        
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
            transform.Position = currentBuilding.transform.position;
            _buildingData.position = transform.Position.xz;
            BuildingManager.Instance.SetBuildingData(_currentId, _buildingData);
            
            Object.Destroy(currentBuilding);
            currentBuilding = null;
        }
        
        public override void OnUpdate()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (currentBuilding == null) return;
            var screenPos = Camera.main.WorldToScreenPoint(currentBuilding.transform.position);
            EventCenter.DispatchEvent(new BuildUIEvent { pos = new Vector3(screenPos[0] - 100, screenPos[1] - 100, 0) });
        }
        
        /// <summary>
        /// 选中某个建筑物后的操作，需要给UI层发事件
        /// 选中后，ecs将模型移动到很远的地方（相当于不渲染模型），原地生成一个透明的GameObject
        /// 选中后的操作和摆放类似
        /// </summary>
        private void SelectBuilding(Entity entity)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var aspect = entityManager.GetAspect<BuildingAspect>(entity);
            _currentId = aspect.ID;
            _buildingData = BuildingManager.Instance.GetBuildingData(_currentId);
            currentBuildingType = aspect.BuildingType;
            var transform = entityManager.GetAspect<TransformAspect>(entity);
            var buildingPos = transform.Position;
            var buildingRot = entityManager.GetAspect<TransformAspect>(aspect.Mesh).Rotation;

            currentBuilding = Object.Instantiate(Data.Config.Instance.GetBuildings()[currentBuildingType], buildingPos, Quaternion.identity);
            currentBuilding.transform.GetChild(0).localRotation = buildingRot;
            MaterialUtil.SetTransparency(currentBuilding, true);
            transform.Position = new float3(10000, 10000, 10000);
        }
        
        private void PlaceBuilding(TouchEvent evt)
        {
            if (currentBuilding == null) return;

            var collisionWorld = BuildingManager.Instance.GetCollisionWorld();
            var raycastInput = BuildingManager.Instance.GetOrCreateRaycastInput(new float3(evt.pos, 0));

            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entity = hit.Entity;
                if (entityManager.HasComponent<BuildingPlane>(entity) && !Managers.Get<IInputManager>().IsPointerOverGameObject())
                {
                    currentBuilding.transform.position = GetBlockPos(hit.Position, out _);
                    UpdateUI();
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float3 GetBlockPos(float3 pos, out int2 gridPos)
        {
            var xzpos = math.floor(pos.xz);
            gridPos = math.int2(xzpos);
            return new float3(xzpos[0], pos.y, xzpos[1]);
        }

        private void RotateBuilding(RotateEvent evt)
        {
            _buildingData.rotation = (_buildingData.rotation + 1) % 4;
            var objTransform = currentBuilding.transform.GetChild(0);
            objTransform.Rotate(Vector3.up, 90);
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var meshRoot = entityManager.GetAspect<BuildingAspect>(buildingEntity).Mesh;

            var transform = entityManager.GetAspect<TransformAspect>(meshRoot);
            transform.LocalRotation = quaternion.RotateY(math.radians(objTransform.localRotation.eulerAngles[1]));
        }
    }
}