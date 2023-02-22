using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Entities;

namespace Game.LevelAndEntity.System
{
    [BurstCompile]
    public partial struct RemoveBlockSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            var removeJob = new RemoveBlockJob
            {
                ECB = ecb.AsParallelWriter(),
            };
            removeJob.Schedule();
        }

        [WithAll(typeof(RemoveBuilding))]
        [BurstCompile]
        partial struct RemoveBlockJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ECB;

            private void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity)
            {
                ECB.DestroyEntity(chunkIndex, entity);
            }
        }

    }
}