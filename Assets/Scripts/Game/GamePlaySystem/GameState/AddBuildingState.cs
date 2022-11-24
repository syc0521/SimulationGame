using System.Runtime.CompilerServices;
using Game.Core;
using Game.Data.Event;
using Game.GamePlaySystem.StateMachine;
using Game.Input;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Config = Game.Data.Config;

namespace Game.GamePlaySystem.GameState
{
    public class AddBuildingState : StateBase
    {
        private int currentBuildingType;
        private GameObject currentBuilding;
        public override void OnEnter(params object[] list)
        {
            currentBuildingType = (int)list[0];
            float3 cameraPos = float3.zero; // todo 改成比较舒服的位置
            currentBuilding = Object.Instantiate(Config.Instance.GetBuildings()[currentBuildingType], cameraPos, Quaternion.identity);
            MaterialUtil.SetTransparency(currentBuilding, true);
            EventCenter.DispatchEvent(new PlaceEvent{pos = currentBuilding.transform.position});
            EventCenter.AddListener<TouchEvent>(PlaceBuilding);
        }

        public override void OnLeave(params object[] list)
        {
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
                    var blockPos = GetBlockPos(hit.Position, out _);
                    currentBuilding.transform.position = blockPos;
                    EventCenter.DispatchEvent(new BuildUIEvent { pos = new Vector3(evt.pos[0], evt.pos[1], 0) });
                }
            }
        }

        /// <summary>
        /// ECS构建建筑物
        /// </summary>
        private void ConstructBuilding()
        {
            var blockPos = GetBlockPos(currentBuilding.transform.position, out var gridPos);
            BuildingManager.Instance.GetGrid().SetData(currentBuildingType, gridPos[0], gridPos[1]);
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AddBlockSystem>().Build(blockPos, currentBuildingType);
            EventCenter.RemoveListener<TouchEvent>(PlaceBuilding);
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