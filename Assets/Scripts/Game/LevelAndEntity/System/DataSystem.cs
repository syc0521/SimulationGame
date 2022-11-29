using Game.Data;
using Game.LevelAndEntity.Aspects;
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
            foreach (var data in SystemAPI.Query<DataAspect>())
            {
                _gameData.people = data.config.ValueRW.people;
            }
        }

        public void GetData(ref GameData gameData)
        {
            gameData = _gameData;
        }
    }
}