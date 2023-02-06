using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using UnityEngine;

namespace Game.GamePlaySystem.Currency
{
    public class CurrencyManager : GamePlaySystemBase<CurrencyManager>
    {
        private Dictionary<int, int> currency;
        public override void OnAwake()
        {
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

        public void SetCurrency(CurrencyType type, int count)
        {
            if (currency.ContainsKey((int)type))
            {
                currency[(int)type] += count;
                // todo 发一个事件 通知UI更新数据
            }
            
            Debug.LogError($"货币类型{type}不存在！");
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