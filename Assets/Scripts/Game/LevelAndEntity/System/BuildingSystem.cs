﻿using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Backpack;
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
            if (!SystemAPI.TryGetSingletonRW(out RefRW<Config> config)) return;
            
            if (!config.ValueRO.dataLoaded)
            {
                InitializeStaticBuilding(ref state);
                config.ValueRW.dataLoaded = true;
            }
            
            int people = 0;
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
                    EventCenter.DispatchEvent(new ProduceEvent
                    {
                        produceType = (ProduceType)data.Producetype,
                        produceID = data.Produceid,
                        count = data.Produceamount[0]
                    });
                    dataChanged = true;
                }
            }
            
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