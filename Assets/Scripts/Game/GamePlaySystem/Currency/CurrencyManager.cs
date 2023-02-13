using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Currency;
using UnityEngine;

namespace Game.GamePlaySystem.Currency
{
    public class CurrencyManager : GamePlaySystemBase<CurrencyManager>
    {
        private Dictionary<int, int> currency;
        public override void OnAwake()
        {
            base.OnAwake();
            EventCenter.AddListener<LoadDataEvent>(InitData);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadDataEvent>(InitData);
            base.OnDestroyed();
        }
        
        private void InitData(LoadDataEvent evt)
        {
            Managers.Get<ISaveDataManager>().GetCurrency(ref currency);
        }

        public void AddCurrency(CurrencyType type, int count)
        {
            if (currency.ContainsKey((int)type))
            {
                currency[(int)type] += count;
                EventCenter.DispatchEvent(new UpdateCurrencyEvent()); // 通知UI更新数据
                return;
            }
            
            Debug.LogError($"货币类型{type}不存在！");
        }

        public bool ConsumeCurrency(CurrencyType type, int count)
        {
            if (currency.ContainsKey((int)type) && currency[(int)type] >= count)
            {
                currency[(int)type] -= count;
                EventCenter.DispatchEvent(new UpdateCurrencyEvent()); // 通知UI更新数据
                return true;
            }

            return false;
        }

        public int GetCurrency(CurrencyType type)
        {
            if (currency.ContainsKey((int)type))
            {
                return currency[(int)type];
            }

            Debug.LogError($"货币类型{type}不存在！");
            return 0;
        }
    }
}