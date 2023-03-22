using System;
using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.GamePlaySystem;
using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Currency;
using Game.UI.ViewData;

namespace Game.UI.UISystem
{
    public class BuildingSystem : UISystemBase<BuildingSystem>
    {
        private Dictionary<int, BuildingViewData> _buildingData = new();
        public override void OnAwake()
        {
            base.OnAwake();
            EventCenter.AddListener<LoadDataEvent>(LoadBuildingData);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadDataEvent>(LoadBuildingData);
            _buildingData.Clear();
            _buildingData = null;
            base.OnDestroyed();
        }

        private void LoadBuildingData(LoadDataEvent evt)
        {
            foreach (var item in ConfigTable.Instance.GetAllBuildingData())
            {
                _buildingData[item.Buildingid] = new BuildingViewData
                {
                    name = item.Name,
                    description = item.Description,
                    itemType = item.Currencytype,
                    itemCount = item.Currencycount,
                    buildingType = (BuildingType)item.Buildingtype + 1
                };
            }
        }

        public Dictionary<int, BuildingViewData> GetAllBuildingViewData() => _buildingData;
        
        public BuildingViewData GetBuildingViewData(int id) => _buildingData.ContainsKey(id) ? _buildingData[id] : null;

        public bool UpgradeBuilding(uint entityId, bool isStatic = false)
        {
            var entityData = BuildingManager.Instance.GetBuildingData(entityId);
            var staticId = entityData.type;
            var buildingData = ConfigTable.Instance.GetBuildingData(staticId);
            var newLevel = entityData.level + 1;
            if (newLevel > buildingData.Level)
            {
                return false;
            }

            var upgradeData = ConfigTable.Instance.GetBuildingUpgradeData(staticId, newLevel);
            if (BackpackManager.Instance.ConsumeBackpack(upgradeData.Itemid, upgradeData.Itemcount) &&
                CurrencyManager.Instance.ConsumeCurrency(upgradeData.Currencyid, upgradeData.Currencycount))
            {
                BuildingManager.Instance.UpgradeBuilding(entityId, newLevel, isStatic);
                return true;
            }

            return false;
        }
    }
}