using System;
using System.Collections.Generic;
using Game.Audio;
using Game.Core;
using Game.Data;
using Game.Data.Shop;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.Shop;
using Game.UI.Component;
using Game.UI.Decorator;
using Game.UI.UISystem;
using Game.UI.Utils;
using Game.UI.ViewData;
using Game.UI.Widget;

namespace Game.UI.Panel.Shop
{
    public class ShopListData : ListData
    {
        public int shopItemId;
        public int staticId;
        public string name;
        public int[] currencyType;
        public int[] currencyCount;
        public int itemCount;
        public Action<int> clickHandler;
    }
    
    public class MallPanel : UIPanel
    {
        public MallPanel_Nodes nodes;
        private ShopItemData _dailyItem;
        private List<ShopItemData> _shopItems;
        
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.back_btn.onClick.AddListener(Close);
            nodes.dailyItem_btn.onClick.AddListener(BuyDailyItem);
        }

        public override void OnShown()
        {
            base.OnShown();
            PlayAnimation();
            GetData();
            ShowDailyItem();
            ShowShopItem();
        }

        public override void OnDestroyed()
        {
            nodes.back_btn.onClick.RemoveListener(Close);
            nodes.dailyItem_btn.onClick.RemoveListener(BuyDailyItem);
            nodes.icon_img.OnDestroyed();
            base.OnDestroyed();
        }
        
        private void Close()
        {
            Managers.Get<IAudioManager>().PlaySFX(SFXType.Button2);
            CloseSelf();
        }

        private void GetData()
        {
            _dailyItem = ShopSystem.Instance.GetDailyShopItem();
            _shopItems = ShopSystem.Instance.GetShopData();
        }

        private void ShowDailyItem()
        {
            var shopData = ConfigTable.Instance.GetStoreItemData(_dailyItem.ShopItemID);
            var buildingData = ConfigTable.Instance.GetBuildingData(shopData.Itemid);
            var isUnlock = BuildingManager.Instance.CheckBuildingUnlocked(shopData.Itemid);
            
            nodes.currency_list.gameObject.SetActive(!isUnlock);
            nodes.finished_go.SetActive(isUnlock);
            nodes.name_txt.text = buildingData.Name;
            nodes.icon_img.SetIcon(IconUtility.GetBuildingIcon(shopData.Itemid));
            nodes.dailyItem_btn.interactable = !BuildingManager.Instance.CheckBuildingUnlocked(shopData.Itemid);

            if (shopData.Consumeid[0] != -1)
            {
                for (var i = 0; i < shopData.Consumeid.Length; i++)
                {
                    nodes.currency_list.AddItem(new ConsumeItemListData
                    {
                        consumeType = ConsumeType.Item,
                        id = shopData.Consumeid[i],
                        amount = shopData.Consumecount[i]
                    });
                }
            }

            if (shopData.Currencyid[0] != -1)
            {
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
        }

        private void BuyDailyItem()
        {
            var shopData = ConfigTable.Instance.GetStoreItemData(_dailyItem.ShopItemID);
            var buildingData = ConfigTable.Instance.GetBuildingData(shopData.Itemid);
            if (BuildingManager.Instance.CheckBuildingUnlocked(shopData.Itemid)) return;
            
            AlertDecorator.OpenAlertPanel($"是否购买{buildingData.Name}？", true, () =>
            {
                ShopManager.Instance.BuyItem(_dailyItem.ShopItemID);
            });
        }
        
        private void ShowShopItem()
        {
            foreach (var itemData in _shopItems)
            {
                var tableData = ConfigTable.Instance.GetStoreItemData(itemData.ShopItemID);
                nodes.shop_list.AddItem(new ShopListData
                {
                    name = tableData.Name,
                    shopItemId = itemData.ShopItemID,
                    staticId = tableData.Itemid,
                    currencyType = tableData.Currencyid,
                    currencyCount = tableData.Currencycount,
                    itemCount = tableData.Count,
                    clickHandler = BuyShopItem
                });
            }
        }
        
        private void BuyShopItem(int shopItemId)
        {
            AlertDecorator.OpenAlertPanel($"是否购买该物品？", true, () =>
            {
                ShopManager.Instance.BuyItem(shopItemId);
            });
        }
    }
}