using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.System
{
    public partial class DataSystem : SystemBase
    {
        private EntityManager entityManager;
        private GameData _gameData;
        private bool dataLoaded = false;

        [BurstCompile]
        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingleton(out Config config)) return;

            if (!dataLoaded)
            {
                LoadData();
                dataLoaded = true;
            }
            
            if (config.dataChanged)
            {
                _gameData = new GameData
                {
                    people = config.people,
                    money = config.money,
                };
                EventCenter.DispatchEvent(new DataChangedEvent { gameData = _gameData });
            }
        }

        private void LoadData()
        {
            int money = Managers.Get<ISaveDataManager>().GetMoney();
            Debug.Log($"LoadData {money}");
            foreach (var data in SystemAPI.Query<DataAspect>())
            {
                data.config.ValueRW.money = money;
            }
        }

    }
}