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
            var configEntity = config[0];
            if (configEntity == Entity.Null) return;
            var buffer = entityManager.GetBuffer<PrefabSpawnerBufferElement>(config[0]);
            Entities.WithAll<AddBuilding>().ForEach((Entity entity, ref AddBuilding addBuilding) =>
            {
                var e = ecb.Instantiate(buffer[addBuilding.spawnType].prefab);
                var position = UniformScaleTransform.FromPosition(addBuilding.spawnPos);
                
                ecb.SetComponent(e, new Translation
                {
                    Value = position.Position
                });
                ecb.AddComponent(e, new LevelObject
                {
                    id = addBuilding.id
                });
                ecb.AddComponent(e, new Timer
                {
                    cd = 3
                });
                
                ecb.DestroyEntity(entity);
            }).Schedule();
            config.Dispose();
            beginSimECBSystem.AddJobHandleForProducer(Dependency);
        }

        public void Build(float3 position, int buildingType, uint id)
        {
            var newBlock = entityManager.CreateEntity();
            entityManager.AddComponentData(newBlock, new AddBuilding
            {
                id = id,
                spawnPos = position,
                spawnType = buildingType
            });
        }
    
    }
}