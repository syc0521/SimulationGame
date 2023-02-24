using Game.Core;
using Game.Data;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.System
{
    public partial class DataSystem : SystemBase
    {

        protected override void OnUpdate()
        {

        }

        public GameData GetGameData()
        {
            var config = GetEntityQuery(typeof(Config)).GetSingletonEntity();
            var aspect = World.EntityManager.GetAspect<DataAspect>(config);
            GameData gameData = new()
            {
                people = aspect.config.ValueRO.people,
                environment = aspect.config.ValueRO.envRate,
                happiness = aspect.config.ValueRO.happiness,
            };
            return gameData;
        }

    }
}