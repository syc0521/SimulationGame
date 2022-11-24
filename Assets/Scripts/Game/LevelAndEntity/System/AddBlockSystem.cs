using Game.LevelAndEntity.Component;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;

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
            var buffer = entityManager.GetBuffer<PrefabSpawnerBufferElement>(config[0]);
            Entities.WithAll<AddBuilding>().ForEach((Entity entity, ref AddBuilding addBuilding) =>
            {
                var e = ecb.Instantiate(buffer[addBuilding.spawnType].prefab);
                var position = UniformScaleTransform.FromPosition(addBuilding.spawnPos);

                ecb.SetComponent(e, new Translation()
                {
                    Value = position.Position
                });
                
                ecb.DestroyEntity(entity);
            }).Schedule();
            config.Dispose();
            beginSimECBSystem.AddJobHandleForProducer(Dependency);
        }

        public void Build(float3 position, int buildingType)
        {
            var newBlock = entityManager.CreateEntity();
            entityManager.AddComponentData(newBlock, new AddBuilding
            {
                spawnPos = position,
                spawnType = buildingType
            });
        }
    
    }
}