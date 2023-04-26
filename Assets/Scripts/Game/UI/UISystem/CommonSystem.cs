using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data;

namespace Game.UI.UISystem
{
    public class CommonSystem : UISystemBase<CommonSystem>
    {
        public List<string> GetLoadingTips()
        {
            var tipsData = ConfigTable.Instance.GetLoadingTipsData();
            var seed = DateTime.Now.Ticks;
            Random random = new((int)seed);
            var ids = new[] { -1, -1, -1, -1 };
            List<string> result = new();
            for (var i = 0; i < 4; i++)
            {
                var id = 0;
                do
                {
                    id = random.Next(0, tipsData.Count);
                } while (ids.Contains(id));

                ids[i] = id;
                result.Add(tipsData[id].Name);
            }
            return result;
        }
    }
}