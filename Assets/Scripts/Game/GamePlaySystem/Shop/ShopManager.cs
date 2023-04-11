using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event.Shop;
using Game.Data.Shop;
using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.Currency;
using UnityEngine;
using Random = System.Random;

namespace Game.GamePlaySystem.Shop
{
    public class ShopManager : GamePlaySystemBase<ShopManager>
    {
        private int _seed;
        private List<ShopItemData> _shopData = new();
        private float _sellRate = 1f;
        public override void OnAwake()
        {
            base.OnAwake();
            GetRandomSeed();
            GetData();
        }

        public override void OnDestroyed()
        {
            _shopData.Clear();
            _shopData = null;
            base.OnDestroyed();
        }

        private void GetRandomSeed()
        {
            var dateNow = DateTime.Now;
            _seed = (int)new DateTime(dateNow.Year, dateNow.Month, dateNow.Day).Ticks;
            Random random = new(_seed);
            _sellRate = random.Next(8, 12) / 10f;
        }

        private void GetData()
        {
            foreach (var itemData in ConfigTable.Instance.GetStoreData())
            {
                _shopData.Add(new ShopItemData
                {
                    ShopItemID = itemData.ID,
                    ShopType = (ShopType)itemData.Storeid
                });
            }
        }

        public List<ShopItemData> GetShopItemData(ShopType type, int count)
        {
            var random = new Random(_seed);
            var shopItemData = new List<ShopItemData>();
            var tmpData = _shopData.Where(item => item.ShopType == type).ToList();
            var shopItemCount = tmpData.Count;
            var randomNumberHistory = new List<int>(15);
            
            if (count > shopItemCount)
            {
                Debug.LogError("商品数过多！");
                return shopItemData;
            }

            for (int i = 0; i < count; i++)
            {
                var index = random.Next(0, shopItemCount);
                while (randomNumberHistory.Contains(index))
                {
                    index = random.Next(0, shopItemCount);
                }
                randomNumberHistory.Add(index);
                shopItemData.Add(tmpData[index]);
            }
            
            randomNumberHistory.Clear();
            return shopItemData;
        }

        public bool BuyItem(int itemID)
        {
            var shopItem = ConfigTable.Instance.GetStoreItemData(itemID);
            if (shopItem == null)
            {
                Debug.LogError($"ID为{itemID}的商品不存在！");
                return false;
            }

            if (BackpackManager.Instance.CheckBackpackItems(shopItem.Consumeid, shopItem.Consumecount) &&
                CurrencyManager.Instance.CheckCurrency(shopItem.Currencyid, shopItem.Currencycount))
            {
                switch ((ShopItemType)shopItem.Itemtype)
                {
                    case ShopItemType.BagItem:
                        BackpackManager.Instance.AddBackpackCount(shopItem.Itemid, shopItem.Count);
                        break;
                    case ShopItemType.Building:
                        BuildingManager.Instance.UnlockBuilding(shopItem.Itemid);
                        break;
                }

                BackpackManager.Instance.ConsumeBackpack(shopItem.Consumeid, shopItem.Consumecount);
                CurrencyManager.Instance.ConsumeCurrency(shopItem.Currencyid, shopItem.Currencycount);
                
                EventCenter.DispatchEvent(new BuySuccessEvent
                {
                    itemID = shopItem.Itemid,
                    type = (ShopItemType)shopItem.Itemtype,
                });
            }
            return false;
        }

        public void SellItem(int itemID, int count)
        {
            var bagItemData = ConfigTable.Instance.GetBagItemData(itemID);
            if (BackpackManager.Instance.GetBackpackCount(itemID) >= count)
            {
                BackpackManager.Instance.ConsumeBackpack(itemID, count);
                CurrencyManager.Instance.AddCurrency(CurrencyType.Coin, (int)(bagItemData.Price * count * _sellRate));
            }
        }
    }
}