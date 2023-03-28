using Game.Data;
using Game.UI.Component;

namespace Game.UI.Utils
{
    public static class IconUtility
    {
        public static AtlasSpriteID GetRewardIcon(RewardType type, int itemId)
        {
            return type switch
            {
                RewardType.Currency => new AtlasSpriteID
                {
                    atlas = AtlasEnum.Currency,
                    resName = $"icon_currency_{itemId}"
                },
                RewardType.Building => new AtlasSpriteID
                {
                    atlas = AtlasEnum.Building,
                    resName = $"icon_building_{itemId}"
                },
                RewardType.Item => new AtlasSpriteID
                {
                    atlas = AtlasEnum.Item,
                    resName = $"icon_item_{itemId}"
                },
                _ => default
            };
        }
    }
}