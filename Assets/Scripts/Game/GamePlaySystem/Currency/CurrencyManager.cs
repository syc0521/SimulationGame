using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Achievement;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Currency;
using Game.GamePlaySystem.Achievement;
using Game.GamePlaySystem.Task;
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
            if (!currency.ContainsKey((int)type))
            {
                currency[(int)type] = count;
            }
            else
            {
                currency[(int)type] += count;
            }
            
            AchievementManager.Instance.TriggerAchievement(AchievementType.Currency, (int)type, count);
            TaskManager.Instance.TriggerTask(TaskType.GetCurrency, (int)type, count);
            EventCenter.DispatchEvent(new UpdateCurrencyEvent()); // 通知UI更新数据
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

            Debug.LogWarning($"货币类型{type}不存在！");
            return 0;
        }

        public bool ConsumeCurrency(int[] id, int[] count)
        {
            if (id.Where((t, i) => !currency.ContainsKey(t) || currency[t] < count[i]).Any())
            {
                return false;
            }
            
            for (int i = 0; i < id.Length; i++)
            {
                ConsumeCurrency((CurrencyType)id[i], count[i]);
            }
            return true;
        }
        
        public bool CheckCurrency(int[] id, int[] count)
        {
            return !id.Where((t, i) => !currency.ContainsKey(t) || currency[t] < count[i]).Any();
        }
        
        public bool CheckCurrency(int id, int count)
        {
            return currency.ContainsKey(id) && currency[id] >= count;
        }
    }
}