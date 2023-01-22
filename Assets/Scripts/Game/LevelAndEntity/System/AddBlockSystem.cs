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
                ecb.AddComponent(e, new BuildingRotation
                {
                    rotation = addBuilding.rotation
                });
                
                ecb.DestroyEntity(entity);
            }).Schedule();
            RotateBuilding();
            config.Dispose();
            beginSimECBSystem.AddJobHandleForProducer(Dependency);
        }

        private void RotateBuilding()
        {
            foreach (var (building, rotation) in SystemAPI.Query<BuildingAspect, BuildingRotation>().WithAll<BuildingRotation>())
            {
                var transform = entityManager.GetAspect<TransformAspect>(building.Mesh);
                transform.LocalRotation = quaternion.RotateY(math.radians(90 * rotation.rotation));
            }
            var ecb = beginSimECBSystem.CreateCommandBuffer();
            Entities.WithAll<BuildingRotation>().ForEach((Entity entity) =>
            {
                ecb.RemoveComponent<BuildingRotation>(entity);
            }).Schedule();
        }

        public void Build(float3 position, int buildingType, uint id, int rotation = 0)
        {
            var newBlock = entityManager.CreateEntity();
            entityManager.AddComponentData(newBlock, new AddBuilding
            {
                id = id,
                spawnPos = position,
                spawnType = buildingType,
                rotation = rotation
            });
        }

    }
}