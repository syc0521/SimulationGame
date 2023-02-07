using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Entities;

namespace Game.LevelAndEntity.System
{
    [BurstCompile]
    public partial struct TurretShootingSystem : ISystem
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
            //transformFromEntity.Update(ref state);
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            var turretShootJob = new TurretShoot
            {
                /*LocalToWorldTransformFromEntity = transformFromEntity,
            ECB = ecb*/
            };
            //turretShootJob.Schedule();
        }
    
        [WithAll(typeof(Shooting))]
        [BurstCompile]
        partial struct TurretShoot : IJobEntity
        {
            /*[ReadOnly] public ComponentLookup<LocalToWorldTransform> LocalToWorldTransformFromEntity;
        public EntityCommandBuffer ECB;*/

            private void Execute(in Aspects.TurretAspect turret)
            {
                /*var instance = ECB.Instantiate(turret.CannonBallPrefab);
            var spawnLocalToWorld = LocalToWorldTransformFromEntity[turret.CannonBallSpawn];
            var ballTransform = UniformScaleTransform.FromPosition(spawnLocalToWorld.Value.Position);

            ballTransform.Scale = LocalToWorldTransformFromEntity[turret.CannonBallPrefab].Value.Scale;
            ECB.SetComponent(instance, new Translation()
            {
                Value = ballTransform.Position
            });
            ECB.SetComponent(instance, new CannonBall
            {
                speed = spawnLocalToWorld.Value.Forward() * 20.0f
            });
            ECB.SetComponent(instance, new URPMaterialPropertyBaseColor
            {
                Value = turret.Color
            });*/
            }
        }
    }
}