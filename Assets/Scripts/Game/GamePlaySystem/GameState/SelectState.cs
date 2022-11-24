using System;
using Game.Core;
using Game.Data.Event;
using Game.GamePlaySystem.StateMachine;
using Game.Input;
using Game.LevelAndEntity.Component;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Game.GamePlaySystem.GameState
{
    public class SelectState : StateBase
    {
        public override void OnEnter(params object[] list)
        {
            EventCenter.AddListener<TouchEvent>(SelectBuilding);
        }

        public override void OnLeave(params object[] list)
        {
            EventCenter.RemoveListener<TouchEvent>(SelectBuilding);
        }
        
        /// <summary>
        /// 选中某个建筑物后的操作，需要给UI层发事件
        /// 选中后，ecs将模型移动到很远的地方（相当于不渲染模型），原地生成一个透明的GameObject
        /// 选中后的操作和摆放类似
        /// </summary>
        private void SelectBuilding(TouchEvent evt)
        {
            var collisionWorld = BuildingManager.Instance.GetCollisionWorld();
            var raycastInput = BuildingManager.Instance.GetOrCreateRaycastInput(new float3(evt.pos, 0));
            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entity = hit.Entity;
                if (entityManager.HasComponent<Building>(entity))
                {
                    EventCenter.DispatchEvent(new SelectEvent
                    {
                        position = entityManager.GetAspect<TransformAspect>(entity).Position
                    });
                }
            }
        }
    }
}