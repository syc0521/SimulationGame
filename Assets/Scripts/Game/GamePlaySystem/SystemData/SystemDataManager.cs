using Game.Core;
using Game.Data;
using Game.Data.Achievement;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Backpack;
using Game.GamePlaySystem.Achievement;
using Game.GamePlaySystem.Backpack;
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
            base.OnAwake();
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
            Managers.Get<ISaveDataManager>().SaveData();
        }

        private void Produce(ProduceEvent evt)
        {
            if (evt.consumeID == -2) // 房屋消耗走单独的表格
            {
                var consumeData = ConfigTable.Instance.GetHouseConsumeData(evt.buildingID, evt.buildingLevel);
                if (!BackpackManager.Instance.ConsumeBackpack(consumeData.Consumeid, consumeData.Produceamount))
                {
                    return;
                }
            }
            else if (evt.consumeID > -1) // 通用消耗逻辑
            {
                if (!BackpackManager.Instance.ConsumeBackpack(evt.consumeID, evt.consumeCount))
                {
                    return;
                }
            }
            
            switch (evt.produceType)
            {
                case ProduceType.Currency: // 货币系统
                    CurrencyManager.Instance.AddCurrency((CurrencyType)evt.produceID, evt.produceCount);
                    break;
                case ProduceType.Item: // 背包系统
                    BackpackManager.Instance.AddBackpackCount(evt.produceID, evt.produceCount);
                    break;
                case ProduceType.Others: // 其他
                    break;
            }
            
            Managers.Get<ISaveDataManager>().SaveData();
        }

        public GameData GetGameData()
        {
            var dataSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DataSystem>();
            if (dataSystem != null)
            {
                var gameData = dataSystem.GetGameData();
                if (!gameData.Equals(default))
                {
                    AchievementManager.Instance.TriggerAchievement(AchievementType.People, -1, gameData.people);
                    return gameData;
                }
            }
            return default;
        }
    }
}