using System;
using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
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
                    currencyType = Array.ConvertAll(item.Currencytype, input => (CurrencyType)input),
                    currencyCount = item.Currencycount,
                    isUnlock = true
                };
            }
        }

        public Dictionary<int, BuildingViewData> GetAllBuildingViewData() => _buildingData;
        
        public BuildingViewData GetBuildingViewData(int id) => _buildingData.ContainsKey(id) ? _buildingData[id] : null;
    }
}