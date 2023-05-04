using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Achievement;
using Game.Data.Event;
using Game.GamePlaySystem.Achievement;
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
            else
            {
                backpack[id] += count;
            }
            
            AchievementManager.Instance.TriggerAchievement(AchievementType.Backpack, id, count);
            TaskManager.Instance.TriggerTask(TaskType.GetBagItem, id, count);
        }
        
        public void AddBackpackCount(int[] id, int[] count)
        {
            for (int i = 0; i < id.Length; i++)
            {
                AddBackpackCount(id[i], count[i]);
            }
        }

        public int GetBackpackCount(int id)
        {
            return backpack.TryGetValue(id, out var value) ? value : 0;
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
        
        public bool CheckBackpackItem(int id, int count)
        {
            return GetBackpackCount(id) >= count;
        }

        public Dictionary<int, int> GetBackpack() => backpack;
    }
}