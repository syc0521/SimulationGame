using System;
using Game.Data;
using Game.Data.Common;
using Game.Data.FeatureOpen;
using Game.Data.GM;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.Currency;
using Game.GamePlaySystem.FeatureOpen;

namespace Game.GamePlaySystem.GM
{
    [GMAttr(type = "一键到位", name = "快冲！", priority = 1)]
    public class OneKeyCommand : ICommand
    {
        public void Run()
        {
            foreach (var item in Enum.GetValues(typeof(CurrencyType)))
            {
                CurrencyManager.Instance.AddCurrency((CurrencyType)item, 10000);
            }
            
            foreach (var item in Enum.GetValues(typeof(FeatureType)))
            {
                FeatureOpenManager.Instance.OpenFeature((FeatureType)item);
            }

            var buildingTableData = ConfigTable.Instance.GetAllBuildingData();
            foreach (var data in buildingTableData)
            {
                BuildingManager.Instance.UnlockBuilding(data.Buildingid);
            }
        }
    }
}