using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.Task;
using UnityEngine;

namespace Game.GamePlaySystem.Backpack
{
    public class BackpackManager : GamePlaySystemBase<BackpackManager>
    {
        private Dictionary<int, int> backpack;
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
            Managers.Get<ISaveDataManager>().GetBackpack(ref backpack);
        }

        public void AddBackpackCount(int id, int count)
        {
            if (!backpack.ContainsKey(id))
            {
                backpack[id] = count;
            }
            backpack[id] += count;
            TaskManager.Instance.TriggerTask(TaskType.GetBagItem, id, count);

            //Debug.Log($"{id}物品数量为{backpack[id]}");
        }

        public int GetBackpackCount(int id)
        {
            if (backpack.ContainsKey(id))
            {
                return backpack[id];
            }
            
            Debug.LogError($"背包物品{id}不存在！");
            return 0;
        }

        public bool ConsumeBackpack(int id, int count)
        {
            if (backpack.ContainsKey(id) && backpack[id] >= count)
            {
                backpack[id] -= count;
                return true;
            }

            return false;
        }

        public bool ConsumeBackpack(int[] id, int[] count)
        {
            if (id.Where((t, i) => !backpack.ContainsKey(t) || backpack[t] < count[i]).Any())
            {
                return false;
            }
            
            for (int i = 0; i < id.Length; i++)
            {
                ConsumeBackpack(id[i], count[i]);
            }
            return true;
        }
        
        public bool CheckBackpackItems(int[] id, int[] count)
        {
            return !id.Where((t, i) => GetBackpackCount(t) < count[i]).Any();
        }

        public Dictionary<int, int> GetBackpack() => backpack;
    }
}