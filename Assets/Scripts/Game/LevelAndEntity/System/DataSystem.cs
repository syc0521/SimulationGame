using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.LevelAndEntity.Aspects;
using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.System
{
    public partial class DataSystem : SystemBase
    {
        private GameData _gameData;

        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingleton(out Config config)) return;

            if (config.dataChanged)
            {
                _gameData = new GameData
                {
                    people = config.people,
                };
                EventCenter.DispatchEvent(new DataChangedEvent { gameData = _gameData });
            }
        }
        
    }
}