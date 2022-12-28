using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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
            int money = 0;
            foreach (var building in SystemAPI.Query<BuildingAspect>().WithAll<Building>())
            {
                people += building.People;
                building.CurrentTime += SystemAPI.Time.DeltaTime;
                if (building.CurrentTime > building.CD)
                {
                    building.CurrentTime = 0;
                    money++;
                }
            }
            
            foreach (var data in SystemAPI.Query<DataAspect>())
            {
                data.config.ValueRW.people = people;
                data.config.ValueRW.money = money;
            }
        }
        
    }
}