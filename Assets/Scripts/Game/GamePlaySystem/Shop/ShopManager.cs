using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event.Common;
using Game.Data.Event.Shop;
using Game.Data.Shop;
using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.Currency;
using Game.GamePlaySystem.Task;
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
                    ShopType = (ShopType)itemData.Storeid,
                    requireLevel = itemData.Requirelevel,
                });
            }
        }

        public List<ShopItemData> GetShopItemData(ShopType type, int count)
        {
            var random = new Random(_seed);
            var shopItemData = new List<ShopItemData>();
            BuildingManager.Instance.TryGetStaticBuildingLevel(10001, out var level);
            var tmpData = _shopData.Where(item => item.ShopType == type && level >= item.requireLevel).ToList();
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

        public void BuyItem(int itemID)
        {
            var shopItem = ConfigTable.Instance.GetStoreItemData(itemID);
            if (shopItem == null)
            {
                Debug.LogError($"ID为{itemID}的商品不存在！");
                EventCenter.DispatchEvent(new AlertTipEvent {tipText = "购买失败！"});
                return;
            }

            if (BackpackManager.Instance.CheckBackpackItems(shopItem.Consumeid, shopItem.Consumecount) &&
                CurrencyManager.Instance.CheckCurrency(shopItem.Currencyid, shopItem.Currencycount))
            {
                RewardType rewardType = RewardType.Item;
                switch ((ShopItemType)shopItem.Itemtype)
                {
                    case ShopItemType.BagItem:
                        rewardType = RewardType.Item;
                        BackpackManager.Instance.AddBackpackCount(shopItem.Itemid, shopItem.Count);
                        TaskManager.Instance.TriggerTask(TaskType.BuyItem, 0);
                        break;
                    case ShopItemType.Building:
                        rewardType = RewardType.Building;
                        BuildingManager.Instance.UnlockBuilding(shopItem.Itemid);
                        TaskManager.Instance.TriggerTask(TaskType.BuyDailyItem, 0);
                        break;
                }

                BackpackManager.Instance.ConsumeBackpack(shopItem.Consumeid, shopItem.Consumecount);
                CurrencyManager.Instance.ConsumeCurrency(shopItem.Currencyid, shopItem.Currencycount);

                var rewardData = new RewardData
                {
                    type = rewardType,
                    itemID = shopItem.Itemid,
                    amount = shopItem.Count,
                };
                EventCenter.DispatchEvent(new BuySuccessEvent
                {
                    RewardData = rewardData
                });
                return;
            }
            EventCenter.DispatchEvent(new AlertTipEvent {tipText = "购买失败！"});
        }

        public void SellItem(int itemID, int count)
        {
            var bagItemData = ConfigTable.Instance.GetBagItemData(itemID);
            if (BackpackManager.Instance.GetBackpackCount(itemID) >= count)
            {
                BackpackManager.Instance.ConsumeBackpack(itemID, count);
                var amount = (int)(bagItemData.Price * count * _sellRate);
                CurrencyManager.Instance.AddCurrency(CurrencyType.Coin, amount);
                TaskManager.Instance.TriggerTask(TaskType.SellItem, 0, count);
                TaskManager.Instance.TriggerTask(TaskType.SellToGetCurrency, 0, amount);
                var rewardData = new RewardData
                {
                    type = RewardType.Currency,
                    itemID = 0,
                    amount = amount,
                };
                EventCenter.DispatchEvent(new SellEvent
                {
                    RewardData = rewardData
                });
            }
        }

        public float GetSellRate() => _sellRate;
    }
}