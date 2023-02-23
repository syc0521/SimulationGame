using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Backpack;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

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
            if (!SystemAPI.TryGetSingletonRW(out RefRW<Config> config)) return;
            
            if (!config.ValueRO.dataLoaded)
            {
                InitializeStaticBuilding(ref state);
                config.ValueRW.dataLoaded = true;
            }
            
            int people = 0;
            bool dataChanged = false;
            int envValue = 115;
            int evaluateScore = 0;
            foreach (var building in SystemAPI.Query<BuildingAspect>().WithAll<Building>())
            {
                var produceData = ConfigTable.Instance.GetBuildingProduceData(building.BuildingType);
                people += building.People; //todo 人口=当前人口数*幸福度
                envValue += building.EnvScore;
                evaluateScore += building.EvaluateScore;
                /*
                 建筑评分B(uilding) = 建筑分总和/该等级需要的分数（该分数对玩家不可见）
                 供给度S(upply) = 所有建筑需要物品总和 * 1.5 > 剩余物品 = 100% 取平均值
                 环境分E(nvironment) = min(100, 115+各建筑环境值总和) / 100
                 幸福度H(appiness) = S * 0.35 + E * 0.4 + B * 0.25
                 人口P(eople) = 当前人口总数（ECS获取） * H
                 以上都是min(1,分数)
                 */
                building.CurrentTime += SystemAPI.Time.DeltaTime;
                var cd = building.CurrentCD;
                if (cd < 0)
                {
                    continue;
                }
                if (building.CurrentTime > cd)
                {
                    building.CurrentTime = 0;
                    EventCenter.DispatchEvent(new ProduceEvent
                    {
                        produceType = (ProduceType)produceData.Producetype,
                        produceID = produceData.Produceid,
                        count = produceData.Produceamount[0]
                    });
                    dataChanged = true;
                }
            }

            float buildRate = math.min(1, evaluateScore / 50.0f);
            float envRate = math.min(100, envValue) / 100.0f;
            float supplyRate = 1.0f;
            float happiness = supplyRate * 0.35f + envRate * 0.4f + buildRate * 0.25f;

            people = (int)(people * happiness);
            foreach (var data in SystemAPI.Query<DataAspect>())
            {
                if (data.config.ValueRO.people != people)
                {
                    data.config.ValueRW.people = people;
                    dataChanged = true;
                }

                data.config.ValueRW.dataChanged = dataChanged;
            }
        }
        
        
        private void InitializeStaticBuilding(ref SystemState state)
        {
            foreach (var building in SystemAPI.Query<StaticBuildingAspect>().WithAll<StaticBuilding>())
            {
                EventCenter.DispatchEvent(new StaticBuildingIntlEvent
                {
                    pos = building.Position,
                    id = building.ID,
                    row = building.Row,
                    col = building.Col,
                });
            }
        }

    }
}