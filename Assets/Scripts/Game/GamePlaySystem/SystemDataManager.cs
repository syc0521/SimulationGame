using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.LevelAndEntity.System;
using Unity.Entities;

namespace Game.GamePlaySystem
{
    public class SystemDataManager : GamePlaySystemBase<SystemDataManager>
    {
        public override void OnAwake()
        {
            EventCenter.AddListener<DataChangedEvent>(ProcessData);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<DataChangedEvent>(ProcessData);
            base.OnDestroyed();
        }

        private void ProcessData(DataChangedEvent evt)
        {
            Managers.Get<ISaveDataManager>().SetMoney(evt.gameData.money);
            Managers.Get<ISaveDataManager>().SaveData();
        }
    }
}