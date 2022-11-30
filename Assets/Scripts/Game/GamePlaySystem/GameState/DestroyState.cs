using System.Numerics;
using Game.Core;
using Game.GamePlaySystem.StateMachine;
using Game.Input;
using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.GamePlaySystem.GameState
{
    public class DestroyState : StateBase
    {
        public override void OnEnter(params object[] list)
        {
            EventCenter.AddListener<TouchEvent>(DeleteBuilding);
        }

        public override void OnLeave(params object[] list)
        {
            EventCenter.RemoveListener<TouchEvent>(DeleteBuilding);
        }
        
        private void DeleteBuilding(TouchEvent evt)
        {
            var collisionWorld = BuildingManager.Instance.GetCollisionWorld();
            var raycastInput = BuildingManager.Instance.GetOrCreateRaycastInput(new float3(evt.pos, 0));
            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entity = hit.Entity;
                if (entityManager.HasComponent<Building>(entity))
                {
                    entityManager.AddComponent<RemoveBuilding>(entity);
                }
            }
        }
    }
}