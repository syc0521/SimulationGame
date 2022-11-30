using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.LevelAndEntity.System
{
    public partial struct BuildingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            
        }

        public void OnDestroy(ref SystemState state)
        {
            
        }
        
        public void OnUpdate(ref SystemState state)
        {
            int people = 0;
            foreach (var building in SystemAPI.Query<BuildingAspect>().WithAll<Building>())
            {
                people += building.People;
            }

            foreach (var data in SystemAPI.Query<DataAspect>())
            {
                data.config.ValueRW.people = people;
            }
        }
        
    }
}