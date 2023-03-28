using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.StateMachine;
using Game.Input;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.GamePlaySystem.GameState
{
    public class DestroyState : StateBase
    {
        public override void OnEnter(params object[] list)
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildingManagedSystem>().SetGridVisible(true);
            EventCenter.AddListener<TouchEvent>(DeleteBuilding);
        }

        public override void OnLeave(params object[] list)
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BuildingManagedSystem>().SetGridVisible(false);
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
                            var buildingUserData = BuildingManager.Instance.GetBuildingData(buildingAspect.ID);
                            BuildingManager.Instance.SetGridData(buildingAspect.Position, buildingUserData.rotation, buildingUserData.type);
                            BuildingManager.Instance.RemoveBuildingData(buildingAspect.ID);
                            entityManager.AddComponent<RemoveBuilding>(entity);
                        }
                    });
                }
            }
        }
    }
}