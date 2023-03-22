using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.Backpack;
using Game.UI.ViewData;

namespace Game.UI.UISystem
{
    public class BackpackSystem : UISystemBase<BackpackSystem>
    {
        public Dictionary<int, BackpackViewData> GetBackpackData()
        {
            var backpackData = new Dictionary<int, BackpackViewData>(3);
            var data = BackpackManager.Instance.GetBackpack();
            foreach (var item in data)
            {
                var tableItem = ConfigTable.Instance.GetBagItemData(item.Key);
                backpackData[item.Key] = new BackpackViewData
                {
                    count = item.Value,
                    name = tableItem.Name,
                    description = tableItem.Content
                };
            }

            return backpackData;
        }
        
    }
}