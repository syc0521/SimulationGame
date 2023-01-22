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
        private GameObject currentBuilding;
        private int rotation = 0;
        private uint currentID = 0;
        public override void OnEnter(params object[] list)
        {
            currentID = BuildingManager.Instance.GetID();
            currentBuildingType = (int)list[0];
            var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.safeArea.width / 2, Screen.safeArea.height / 2, 0));
            var point = ray.origin - ray.direction * (ray.origin.y / ray.direction.y);

            currentBuilding = Object.Instantiate(ConfigTable.Instance.GetBuilding(currentBuildingType), GetBlockPos(point, out _), Quaternion.identity);
            MaterialUtil.SetTransparency(currentBuilding, true);
            EventCenter.AddListener<TouchEvent>(PlaceBuilding);
            EventCenter.AddListener<RotateEvent>(RotateBuilding);
        }

        public override void OnUpdate()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (currentBuilding == null || Camera.main == null) return;
            var screenPos = Camera.main.WorldToScreenPoint(currentBuilding.transform.position);
            EventCenter.DispatchEvent(new BuildUIEvent { pos = new Vector3(screenPos[0] - 100, screenPos[1] - 100, 0) });
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

        /// <summary>
        /// ECS构建建筑物
        /// </summary>
        private void ConstructBuilding()
        {
            var pos = currentBuilding.transform.position;
            BuildingManager.Instance.SetBuildingData(currentID, new BuildingData
            {
                level = 1,
                position = ((float3)pos).xz,
                rotation = rotation,
                type = currentBuildingType
            });
            
            var blockPos = GetBlockPos(pos, out var gridPos);
            BuildingManager.Instance.GetGrid().SetData(currentBuildingType, gridPos[0], gridPos[1]);
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AddBlockSystem>().Build(blockPos, currentBuildingType, currentID, rotation);
            EventCenter.RemoveListener<TouchEvent>(PlaceBuilding);
            TaskManager.Instance.TriggerTask(TaskType.AddBuilding, currentBuildingType, 1);
        }
        
        private void RotateBuilding(RotateEvent evt)
        {
            rotation = (rotation + 1) % 4;
            var objTransform = currentBuilding.transform.GetChild(0);
            objTransform.Rotate(Vector3.up, 90);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float3 GetBlockPos(float3 pos, out int2 gridPos)
        {
            var xzpos = math.floor(pos.xz);
            gridPos = math.int2(xzpos);
            return new float3(xzpos[0], pos.y, xzpos[1]);
        }
    }
}