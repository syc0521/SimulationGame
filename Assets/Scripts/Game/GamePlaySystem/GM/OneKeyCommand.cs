using System;
using System.Linq;
using Game.Data;
using Game.Data.Common;
using Game.Data.FeatureOpen;
using Game.Data.GM;
using Game.GamePlaySystem.Backpack;
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
            BackpackManager.Instance.AddBackpackCount(0, 5000);
            BackpackManager.Instance.AddBackpackCount(1, 5000);
            BackpackManager.Instance.AddBackpackCount(2, 5000);
            BackpackManager.Instance.AddBackpackCount(10, 5000);
            BackpackManager.Instance.AddBackpackCount(11, 5000);
            
            foreach (var item in Enum.GetValues(typeof(CurrencyType)))
            {
                CurrencyManager.Instance.AddCurrency((CurrencyType)item, 10000);
            }
            
            var featureBlackList = new[]{ FeatureType.Chapter1, FeatureType.Chapter2, FeatureType.Chapter3 };

            foreach (var item in Enum.GetValues(typeof(FeatureType)))
            {
                var feature = (FeatureType)item;
                if (!featureBlackList.Contains(feature))
                {
                    FeatureOpenManager.Instance.OpenFeature((FeatureType)item);
                }
            }

            var blackList = new[]{ 1, 19, 20 };
            var buildingData = ConfigTable.Instance.GetAllBuildingData();
            foreach (var data in buildingData.Where(item => !blackList.Contains(item.Buildingid)))
            {
                BuildingManager.Instance.UnlockBuilding(data.Buildingid);
            }
        }
    }
}