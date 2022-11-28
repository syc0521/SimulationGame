using Game.Data;
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

        protected override void OnUpdate()
        {
            
        }

        public void GetData(out GameData gameData)
        {
            gameData = _gameData;
        }
    }
}