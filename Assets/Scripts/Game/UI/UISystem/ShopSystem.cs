using System.Collections.Generic;
using System.Linq;
using Game.Data.Shop;
using Game.GamePlaySystem.Shop;

namespace Game.UI.UISystem
{
    public class ShopSystem : UISystemBase<ShopSystem>
    {
        public List<ShopItemData> GetShopData()
        {
            return ShopManager.Instance.GetShopItemData(ShopType.Mall, 3);
        }

        public ShopItemData GetDailyShopItem()
        {
            return ShopManager.Instance.GetShopItemData(ShopType.BuildingShop, 1)[0];
        }
    }
}