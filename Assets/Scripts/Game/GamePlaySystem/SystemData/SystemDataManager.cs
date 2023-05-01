using System;
using Game.Core;
using Game.Data;
using Game.Data.Achievement;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Backpack;
using Game.GamePlaySystem.Achievement;
using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.Currency;
using Game.GamePlaySystem.Task;
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
                    TaskManager.Instance.TriggerTask(TaskType.GetEvaluateScore, 0, (int)(gameData.buildingRate * 100));
                    TaskManager.Instance.TriggerTask(TaskType.People, 0, gameData.people);
                    AchievementManager.Instance.TriggerAchievement(AchievementType.People, -1, gameData.people);
                    return gameData;
                }
            }
            return default;
        }

        public void CalculateOfflineData()
        {
            // 处理放置生产：离线只能产生10%的收益
            var userData = BuildingManager.Instance.GetAllBuildingData();
            var lastLoginTime = Managers.Get<ISaveDataManager>().GetLastLoginTime().ToUniversalTime();
            var deltaTime = DateTime.Now.ToUniversalTime() - lastLoginTime;
            var deltaMinute = deltaTime.Minutes;
            foreach (var (_, data) in userData)
            {
                var staticId = data.type;
                var level = data.level;
                var buildingData = ConfigTable.Instance.GetBuildingData(staticId);
                
                if (buildingData.Cd > 0) // 可生产
                {
                    var produceData = ConfigTable.Instance.GetBuildingProduceData(staticId);
                    var produceCount = Mathf.CeilToInt(produceData.Produceamount[level - 1] / buildingData.Cd * 60.0f * deltaMinute * 0.1f);
                    int consumeCount = 0;
                    if (produceData.Consumeid > 0)
                    {
                        consumeCount = Mathf.CeilToInt(produceData.Consumeamount[level - 1] / buildingData.Cd * 60.0f * deltaMinute * 0.1f);
                    }
                    EventCenter.DispatchEvent(new ProduceEvent
                    {
                        produceType = (ProduceType)produceData.Producetype,
                        produceID = produceData.Produceid,
                        produceCount = produceCount,
                        consumeID = produceData.Consumeid,
                        consumeCount = consumeCount,
                        buildingID = staticId,
                        buildingLevel = level,
                    });
                }
            }
        }
    }
}