using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Game.LevelAndEntity.System
{
    [BurstCompile]
    public partial struct TankSpawningSystem : ISystem
    {
        private EntityQuery baseColorQuery;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            //baseColorQuery = state.GetEntityQuery(ComponentType.ReadOnly<URPMaterialPropertyBaseColor>());
        }
    
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            /*var config = SystemAPI.GetSingleton<Config>();
            var random = Random.CreateFromIndex(1234);
            var hue = random.NextFloat();

            URPMaterialPropertyBaseColor RandomColor()
            {
                hue = (hue + 0.618034005f) % 1;
                var color = Color.HSVToRGB(hue, 1.0f, 1.0f);
                return new URPMaterialPropertyBaseColor { Value = (Vector4)color };
            }
        
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            var vehicles = CollectionHelper.CreateNativeArray<Entity>(config.tankCount, Allocator.Temp);
            ecb.Instantiate(config.tankPrefab, vehicles);

            var queryMask = baseColorQuery.GetEntityQueryMask();
            foreach (var vehicle in vehicles)
            {
                ecb.SetComponentForLinkedEntityGroup(vehicle, queryMask, RandomColor());
            }
            state.Enabled = false;*/
        }
    }
}
