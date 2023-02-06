using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using UnityEngine;

namespace Game.GamePlaySystem.Backpack
{
    public class BackpackManager : GamePlaySystemBase<BackpackManager>
    {
        private Dictionary<int, int> backpack;
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
            Managers.Get<ISaveDataManager>().GetBackpack(ref backpack);
        }

        public void AddBackpackCount(int id, int count)
        {
            if (!backpack.ContainsKey(id))
            {
                backpack[id] = count;
            }
            backpack[id] += count;
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

        public Dictionary<int, int> GetBackpack() => backpack;
    }
}