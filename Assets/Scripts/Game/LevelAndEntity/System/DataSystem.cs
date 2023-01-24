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
        private EndSimulationEntityCommandBufferSystem ecbSystem;
        private EntityManager entityManager;
        private GameData _gameData;

        protected override void OnCreate()
        {
            ecbSystem = World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var config = SystemAPI.GetSingleton<Config>();
            _gameData.people = config.people;
            _gameData.money = config.money;
            if (config.dataChanged)
            {
                EventCenter.DispatchEvent(new DataChangedEvent { gameData = _gameData });
            }
        }

        public void GetData(ref GameData gameData)
        {
            gameData = _gameData;
        }
    }
}