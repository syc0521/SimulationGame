using Game.LevelAndEntity.Component;
using Unity.Entities;

namespace Game.LevelAndEntity.System
{
    public partial class RemoveBlockSystem : SystemBase
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

            Entities.WithAll<RemoveBuilding>().ForEach((Entity entity) =>
            {
                ecb.DestroyEntity(entity);
            }).WithoutBurst().Run();
        }
    }
}