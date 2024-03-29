﻿using Game.Data;
using Game.UI.Component;

namespace Game.UI.Utils
{
    public static class IconUtility
    {
        public static AtlasSpriteID GetRewardIcon(RewardType type, int itemId)
        {
            return type switch
            {
                RewardType.Currency => GetCurrencyIcon(itemId),
                RewardType.Building => GetBuildingIcon(itemId),
                RewardType.Item => GetItemIcon(itemId),
                _ => default
            };
        }
        
        public static AtlasSpriteID GetBuildingIcon(int buildingId)
        {
            return new AtlasSpriteID
            {
                atlas = AtlasEnum.Building,
                resName = $"icon_building_{buildingId}"
            };
        }
        
        public static AtlasSpriteID GetItemIcon(int itemId)
        {
            var itemName = ConfigTable.Instance.GetBagItemData(itemId).Iconname;
            return new AtlasSpriteID
            {
                atlas = AtlasEnum.Item,
                resName = $"icon_item_{itemName}"
            };
        }
        
        public static AtlasSpriteID GetCurrencyIcon(int currencyId)
        {
            return new AtlasSpriteID
            {
                atlas = AtlasEnum.Currency,
                resName = $"icon_currency_{currencyId}"
            };
        }

        public static AtlasSpriteID GetTaskIcon(int taskId)
        {
            return new AtlasSpriteID
            {
                atlas = AtlasEnum.Task,
                resName = $"icon_task_{taskId}"
            };
        }
        
        public static AtlasSpriteID GetFeatureIcon(int featureId)
        {
            return new AtlasSpriteID
            {
                atlas = AtlasEnum.Feature,
                resName = $"icon_feature_{featureId}"
            };
        }
    }
}