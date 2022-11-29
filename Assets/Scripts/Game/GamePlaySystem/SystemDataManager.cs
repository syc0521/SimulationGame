using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.LevelAndEntity.System;
using Unity.Entities;

namespace Game.GamePlaySystem
{
    public class SystemDataManager : GamePlaySystemBase<SystemDataManager>
    {
        public ValueChangedEvent<int> People { get; private set; }
        private GameData _gameData;
        public override void OnStart()
        {
            People = new(OnPeopleChange);
        }

        public override void OnUpdate()
        {
            var dataSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DataSystem>();
            dataSystem.GetData(ref _gameData);
            People.Value = _gameData.people;
        }

        private void OnPeopleChange(int people)
        {
            EventCenter.DispatchEvent(new DataChangedEvent { gameData = _gameData });
        }
    }
}