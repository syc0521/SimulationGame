using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Burst;
using Unity.Entities;

namespace Game.LevelAndEntity.System
{
    public partial class DataSystem : SystemBase
    {
        private EntityManager entityManager;
        private GameData _gameData;

        protected override void OnStartRunning()
        {
            int money = Managers.Get<ISaveDataManager>().GetMoney();
            var config = SystemAPI.GetSingleton<Config>();
            config.money = money;
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingleton(out Config config)) return;
            
            _gameData.people = config.people;
            _gameData.money = config.money;
            if (config.dataChanged)
            {
                EventCenter.DispatchEvent(new DataChangedEvent { gameData = _gameData });
            }
        }

    }
}