﻿using Game.Core;
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
            NativeArray<int> output = new NativeArray<int>(3, Allocator.Persistent);
            NativeList<int> produce = new NativeList<int>(state.WorldUpdateAllocator);
            var deltaTime = SystemAPI.Time.DeltaTime;
            var buildingJob = new BuildingJob
            {
                output = output,
                produce = produce,
                deltaTime = deltaTime
            };

            buildingJob.Schedule();
            state.Dependency.Complete();

            foreach (var item in produce)
            {
                var produceData = ConfigTable.Instance.GetBuildingProduceData(item);
                EventCenter.DispatchEvent(new ProduceEvent
                {
                    produceType = (ProduceType)produceData.Producetype,
                    produceID = produceData.Produceid,
                    count = produceData.Produceamount[0]
                });
            }
            produce.Dispose();

            float buildRate = math.min(1, output[2] / 50.0f);
            float envRate = math.min(100, output[1] + 115) / 100.0f;
            float supplyRate = 1.0f;
            float happiness = supplyRate * 0.35f + envRate * 0.4f + buildRate * 0.25f;
            
            config.ValueRW.people = (int)(output[0] * happiness);
            config.ValueRW.envRate = envRate;
            config.ValueRW.happiness = happiness;
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

    }

    [WithAll(typeof(Building))]
    [BurstCompile]
    partial struct BuildingJob : IJobEntity
    {
        public NativeArray<int> output;
        public NativeList<int> produce;
        public float deltaTime;
        private void Execute(BuildingAspect building)
        {
            output[0] += building.People;
            output[1] += building.EnvScore;
            output[2] += building.EvaluateScore;
            building.CurrentTime += deltaTime;
            if (building.CurrentCD > 0 && building.CurrentTime > building.CurrentCD)
            {
                building.CurrentTime = 0;
                produce.Add(building.BuildingType);
            }
        }
    }
}