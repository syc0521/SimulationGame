using Game.Data;
using Game.Data.Shop;
using Game.GamePlaySystem.Build;
using Game.UI.Decorator;
using Game.UI.UISystem;
using Game.UI.Utils;
using Game.UI.ViewData;
using Game.UI.Widget;

namespace Game.UI.Panel.Shop
{
    public class MallPanel : UIPanel
    {
        public MallPanel_Nodes nodes;
        private ShopItemData _dailyItem;
        
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.back_btn.onClick.AddListener(CloseSelf);
            nodes.dailyItem_btn.onClick.AddListener(BuyDailyItem);
        }

        public override void OnShown()
        {
            base.OnShown();
            ShowDailyItem();
        }

        public override void OnDestroyed()
        {
            nodes.back_btn.onClick.RemoveListener(CloseSelf);
            nodes.dailyItem_btn.onClick.RemoveListener(BuyDailyItem);
            base.OnDestroyed();
        }

        private void ShowDailyItem()
        {
            _dailyItem = ShopSystem.Instance.GetDailyShopItem();
            var shopData = ConfigTable.Instance.GetStoreItemData(_dailyItem.ShopItemID);
            var buildingData = ConfigTable.Instance.GetBuildingData(shopData.Itemid);
            nodes.name_txt.text = buildingData.Name;
            nodes.icon_img.SetIcon(IconUtility.GetBuildingIcon(shopData.Itemid));
            nodes.dailyItem_btn.interactable = !BuildingManager.Instance.CheckBuildingUnlocked(shopData.Itemid);
            
            for (var i = 0; i < shopData.Consumeid.Length; i++)
            {
                nodes.currency_list.AddItem(new ConsumeItemListData
                {
                    consumeType = ConsumeType.Item,
                    id = shopData.Consumeid[i],
                    amount = shopData.Consumecount[i]
                });
            }
            for (var i = 0; i < shopData.Currencyid.Length; i++)
            {
                nodes.currency_list.AddItem(new ConsumeItemListData
                {
                    consumeType = ConsumeType.Currency,
                    id = shopData.Currencyid[i],
                    amount = shopData.Currencycount[i]
                });
            }
        }

        private void BuyDailyItem()
        {
            var shopData = ConfigTable.Instance.GetStoreItemData(_dailyItem.ShopItemID);
            var buildingData = ConfigTable.Instance.GetBuildingData(shopData.Itemid);
            if (BuildingManager.Instance.CheckBuildingUnlocked(shopData.Itemid)) return;
            
            AlertDecorator.OpenAlertPanel($"是否购买{buildingData.Name}？", true, () =>
            {
                
            });
        }
    }
}