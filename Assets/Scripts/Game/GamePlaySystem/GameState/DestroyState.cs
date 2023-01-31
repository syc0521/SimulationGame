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
            Managers.Get<ISaveDataManager>().SaveData();
            EventCenter.RemoveListener<TouchEvent>(DeleteBuilding);
        }

        private void DeleteBuilding(TouchEvent evt)
        {
            var collisionWorld = BuildingManager.Instance.GetCollisionWorld();
            var raycastInput = BuildingManager.Instance.GetRaycastInput(new float3(evt.pos, 0));
            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var entity = hit.Entity;
                if (entityManager.HasComponent<Building>(entity))
                {
                    EventCenter.DispatchEvent(new DestroyEvent
                    {
                        handler = () =>
                        {
                            var buildingAspect = entityManager.GetAspect<BuildingAspect>(entity);
                            BuildingManager.Instance.RemoveBuildingData(buildingAspect.ID);
                            entityManager.AddComponent<RemoveBuilding>(entity);

                            var transform = entityManager.GetAspect<TransformAspect>(entity);
                            var buildingUserData = BuildingManager.Instance.GetBuildingData(buildingAspect.ID);
                            BuildingManager.Instance.SetGridData(transform.Position, buildingUserData.rotation, buildingUserData.type);
                        }
                    });
                }
            }
        }
    }
}