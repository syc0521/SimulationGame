using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.LevelAndEntity.System
{
    [BurstCompile]
    public partial struct CannonBallSystem : ISystem
    {
        //private ComponentLookup<LocalToWorldTransform> transformFromEntity;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            //transformFromEntity = state.GetComponentLookup<LocalToWorldTransform>(true);
        }
    
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            /*transformFromEntity.Update(ref state);
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var cannonBallJob = new CannonBallJob
        {
            ECB = ecb.AsParallelWriter(),
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        cannonBallJob.Schedule();*/
        }
    
        [BurstCompile]
        partial struct CannonBallJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ECB;
            public float DeltaTime;

            private void Execute([ChunkIndexInQuery] int chunkIndex, ref Aspects.CannonBallAspect cannonBall)
            {
                var gravity = new float3(0, -9.82f, 0);
                var invertY = new float3(1.0f, -1.0f, 1.0f);

                cannonBall.Position += cannonBall.Speed * DeltaTime;
                if (cannonBall.Position.y < 0)
                {
                    cannonBall.Position *= invertY;
                    cannonBall.Speed *= invertY * 0.8f;
                }

                cannonBall.Speed *= gravity * DeltaTime;

                var speed = math.lengthsq(cannonBall.Speed);
                if (speed < 0.02f)
                {
                    ECB.DestroyEntity(chunkIndex, cannonBall.self);
                }
            }
        }
    }
}