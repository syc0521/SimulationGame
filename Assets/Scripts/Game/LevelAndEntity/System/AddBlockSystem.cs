using Game.LevelAndEntity.Component;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.LevelAndEntity.System
{
    public partial class AddBlockSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem beginSimECBSystem;
        private EntityManager entityManager;

        protected override void OnCreate()
        {
            beginSimECBSystem = World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
            entityManager = beginSimECBSystem.EntityManager;
        }
        
        protected override void OnUpdate()
        {
            var ecb = beginSimECBSystem.CreateCommandBuffer();
            var query = GetEntityQuery(typeof(Config));
            var config = query.ToEntityArray(Allocator.Temp);
            if (config.Length <= 0) return;

            var configEntity = config[0];
            if (configEntity == Entity.Null) return;

            var buffer = entityManager.GetBuffer<PrefabSpawnerBufferElement>(config[0]);
            
            Entities.WithAll<AddBuilding>().ForEach((Entity entity, ref AddBuilding addBuilding) =>
            {
                var e = ecb.Instantiate(buffer[addBuilding.spawnType].prefab);

                ecb.SetComponent(e, new LocalTransform
                {
                    Position = addBuilding.spawnPos + addBuilding.offset,
                    Rotation = quaternion.RotateY(math.radians(90 * addBuilding.rotation)),
                    Scale = 1
                });

                ecb.AddComponent(e, new LevelObject
                {
                    id = addBuilding.id,
                    level = 1,
                    spawnPos = addBuilding.spawnPos,
                });
                ecb.AddComponent(e, new Timer());

                ecb.DestroyEntity(entity);
            }).Schedule();
            config.Dispose();
            beginSimECBSystem.AddJobHandleForProducer(Dependency);
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

    }
}