using Game.LevelAndEntity.Component;
using Unity.Collections;
using Unity.Entities;

namespace Game.LevelAndEntity.System
{
    public partial class BuildingManagedSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem beginSimECBSystem;
        private EntityManager entityManager;

        protected override void OnCreate()
        {
            base.OnCreate();
            beginSimECBSystem = World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
            entityManager = beginSimECBSystem.EntityManager;
        }

        protected override void OnUpdate()
        {
            
        }
        
        public void SetGridVisible(bool b)
        {
            var query = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]{typeof(BuildingPlane)},
                Options = EntityQueryOptions.IncludeDisabledEntities
            });
            var planeEntity = query.GetSingletonEntity();
            entityManager.SetEnabled(planeEntity, b);
        }
        
        public void UpgradeBuilding(uint buildingId, int newLevel, bool isStatic = false)
        {
            //var ecb = beginSimECBSystem.CreateCommandBuffer();
            Entities.WithAll<LevelObject>().WithAny<Building, StaticBuilding>().ForEach((Entity entity, ref LevelObject levelObject) =>
            {
                if (levelObject.isStatic == isStatic && levelObject.id == buildingId)
                {
                    levelObject.level = newLevel;
                }
            }).Schedule();
            beginSimECBSystem.AddJobHandleForProducer(Dependency);
        }

        public int GetStaticBuildingLevel(uint buildingId)
        {
            var level = 0;
            Entities.WithAll<LevelObject>().WithAny<Building, StaticBuilding>().ForEach((Entity entity, ref LevelObject levelObject) =>
            {
                if (levelObject.isStatic && levelObject.id == buildingId)
                {
                    level = levelObject.level;
                }
            }).Schedule();
            beginSimECBSystem.AddJobHandleForProducer(Dependency);
            
            return level;
        }
    }
}