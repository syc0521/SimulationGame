using Game.Core;
using Game.Data;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.ResLoader;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
                var position = UniformScaleTransform.FromPosition(addBuilding.spawnPos + addBuilding.offset);
                
                ecb.SetComponent(e, new Translation
                {
                    Value = position.Position
                });
                ecb.SetComponent(e, new Rotation
                {
                    Value = quaternion.RotateY(math.radians(90 * addBuilding.rotation))
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

    }
}