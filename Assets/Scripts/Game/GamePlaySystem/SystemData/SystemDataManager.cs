using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Backpack;
using Game.GamePlaySystem.Currency;
using Game.LevelAndEntity.System;
using Unity.Entities;
using UnityEngine;

namespace Game.GamePlaySystem
{
    public class SystemDataManager : GamePlaySystemBase<SystemDataManager>
    {
        public override void OnAwake()
        {
            EventCenter.AddListener<DataChangedEvent>(ProcessData);
            EventCenter.AddListener<ProduceEvent>(Produce);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<DataChangedEvent>(ProcessData);
            EventCenter.RemoveListener<ProduceEvent>(Produce);
            base.OnDestroyed();
        }

        private void ProcessData(DataChangedEvent evt)
        {
            Debug.Log(evt.gameData.money);
            Managers.Get<ISaveDataManager>().SaveData();
        }

        private void Produce(ProduceEvent evt)
        {
            switch (evt.produceType)
            {
                case ProduceType.Currency: // 货币系统
                    CurrencyManager.Instance.SetCurrency((CurrencyType)evt.produceID, evt.count);
                    break;
                case ProduceType.Item:
                    // 背包系统
                    break;
                case ProduceType.Others:
                    // 其他
                    break;
            }
        }
    }
}