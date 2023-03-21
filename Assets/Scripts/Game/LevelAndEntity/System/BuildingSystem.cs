using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Backpack;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Game.LevelAndEntity.System
{
    [BurstCompile]
    [UpdateAfter(typeof(AddBlockSystem))]
    public partial struct BuildingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
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
            
            /*
                建筑评分B(uilding) = 建筑分总和/该等级需要的分数（该分数对玩家不可见）
                供给度S(upply) = 所有建筑需要物品总和 * 1.5 > 剩余物品 = 100% 取平均值
                环境分E(nvironment) = min(100, 115+各建筑环境值总和) / 100
                幸福度H(appiness) = S * 0.35 + E * 0.4 + B * 0.25
                人口P(eople) = 当前人口总数（ECS获取） * H
                以上都是min(1,分数)
            */
            NativeArray<int> output = new NativeArray<int>(3, Allocator.TempJob);
            NativeList<int2> produce = new(1, state.WorldUpdateAllocator);
            NativeList<int2> buildings = new(1, state.WorldUpdateAllocator);
            var deltaTime = SystemAPI.Time.DeltaTime;
            var buildingJob = new BuildingJob
            {
                output = output,
                produce = produce,
                deltaTime = deltaTime,
                buildings = buildings,
            };

            buildingJob.Schedule();
            state.Dependency.Complete();

            foreach (var item in produce)
            {
                var produceData = ConfigTable.Instance.GetBuildingProduceData(item[0]);
                EventCenter.DispatchEvent(new ProduceEvent
                {
                    produceType = (ProduceType)produceData.Producetype,
                    produceID = produceData.Produceid,
                    produceCount = produceData.Produceamount[item[1] - 1],
                    consumeID = produceData.Consumeid,
                    consumeCount = produceData.Consumeid > 0 ? produceData.Consumeamount[item[1] - 1] : 0,
                    buildingID = item[0],
                    buildingLevel = item[1],
                });
            }
            produce.Dispose();

            NativeList<int2> consumeItems = new(1, state.WorldUpdateAllocator);
            foreach (var item in buildings)
            {
                var produceData = ConfigTable.Instance.GetBuildingProduceData(item[0]);
                if (produceData.Consumeid == -2)
                {
                    var houseConsume = ConfigTable.Instance.GetHouseConsumeData(item[0], item[1]);
                    for (int i = 0; i < houseConsume.Consumeid.Length; i++)
                    {
                        consumeItems.Add(new(houseConsume.Consumeid[i], (int)(houseConsume.Produceamount[i] * 1.5f)));
                    }
                }
                else if (produceData.Consumeid != -1)
                {
                    consumeItems.Add(new(produceData.Consumeid, (int)(produceData.Consumeamount[item[1] - 1] * 1.5f)));
                }
            }

            float buildRate = math.min(1, output[2] / 50.0f);
            float envRate = math.min(100, output[1] + 115) / 100.0f;
            float supplyRate = GetSupplyRate(consumeItems);
            float happiness = supplyRate * 0.35f + envRate * 0.4f + buildRate * 0.25f;

            config.ValueRW.people = (int)math.ceil(output[0] * happiness);
            config.ValueRW.envRate = envRate;
            config.ValueRW.happiness = happiness;
            config.ValueRW.buildingRate = buildRate;
            buildings.Dispose();
            consumeItems.Dispose();
            output.Dispose();
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
        
        private float GetSupplyRate(in NativeList<int2> consumeItems)
        {
            Dictionary<int, int> backpack = new(0);
            Managers.Get<ISaveDataManager>().GetBackpack(ref backpack);
            
            var rate = 0f;
            int count = 0;
            foreach (var item in consumeItems)
            {
                if (backpack.ContainsKey(item[0]))
                {
                    rate += math.min(1f, (float)backpack[item[0]] / item[1]);
                    count++;
                }
            }
            
            return count > 0 ? rate / count : 1f;
        }

    }

    [WithAll(typeof(Building))]
    [BurstCompile]
    partial struct BuildingJob : IJobEntity
    {
        public NativeArray<int> output;
        public NativeList<int2> produce;
        public NativeList<int2> buildings;
        public float deltaTime;
        private void Execute(BuildingAspect building)
        {
            output[0] += building.People;
            output[1] += building.EnvScore;
            output[2] += building.EvaluateScore;
            buildings.Add(new(building.BuildingType, building.Level));
            building.CurrentTime += deltaTime;
            if (building.CurrentCD > 0 && building.CurrentTime > building.CurrentCD)
            {
                building.CurrentTime = 0;
                produce.Add(new(building.BuildingType, building.Level));
            }
        }
    }
}