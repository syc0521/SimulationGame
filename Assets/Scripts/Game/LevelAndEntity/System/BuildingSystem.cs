using Game.Data;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Burst;
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
            var config = SystemAPI.GetSingleton<Config>();
            int people = 0;
            int money = config.money;
            bool dataChanged = false;
            foreach (var building in SystemAPI.Query<BuildingAspect>().WithAll<Building>())
            {
                var data = ConfigTable.Instance.GetBuildingData(building.BuildingType);
                people += building.People; //todo 人口=当前人口数*幸福度
                building.CurrentTime += SystemAPI.Time.DeltaTime;
                var cd = data.Cd[building.Level - 1];
                if (cd < 0)
                {
                    continue;
                }
                if (building.CurrentTime > cd)
                {
                    building.CurrentTime = 0;
                    money += data.Produceamount[0]; // todo 以后不用钱，改成建筑材料
                    dataChanged = true;
                }
            }
            
            foreach (var data in SystemAPI.Query<DataAspect>())
            {
                if (data.config.ValueRW.people != people)
                {
                    data.config.ValueRW.people = people;
                    dataChanged = true;
                }
                
                // todo 换成背包系统
                if (data.config.ValueRW.money != money)
                {
                    data.config.ValueRW.money = money;
                    dataChanged = true;
                }
                
                data.config.ValueRW.dataChanged = dataChanged;
            }
        }
        
    }
}