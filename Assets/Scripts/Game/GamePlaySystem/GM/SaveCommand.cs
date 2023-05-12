using System.IO;
using System.Text;
using Game.Core;
using Game.Data.Event.Common;
using Game.Data.GM;
using Game.GamePlaySystem.Build;
using UnityEngine;

namespace Game.GamePlaySystem.GM
{
    [GMAttr(type = "存档", name = "导出建筑信息", priority = 1)]
    public class ExportSaveCommand : ICommand
    {
        public void Run()
        {
            var outStr = new StringBuilder();
            var buildingData = BuildingManager.Instance.GetAllBuildingData();
            foreach (var data in buildingData.Values)
            {
                outStr.AppendLine($"BuildingManager.Instance.QuickBuild({data.type}, {data.position.x}, {data.position.y}, {data.rotation});");
            }

            StreamWriter sw = new("D:/outsave.txt");
            sw.Write(outStr.ToString());
            sw.Close();
            sw.Dispose();
        }
    }
    
    
    [GMAttr(type = "存档", name = "清除存档（需重启）", priority = 2)]
    public class ClearSaveCommand : ICommand
    {
        public void Run()
        {
            EventCenter.DispatchEvent(new ClearSaveEvent());
        }
    }
    
    [GMAttr(type = "存档", name = "1号城市", priority = 3)]
    public class QuickBuild1Command : ICommand
    {
        public void Run()
        {
            BuildingManager.Instance.QuickBuild(11, 115, 115, 0);
            BuildingManager.Instance.QuickBuild(4, 123, 118, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 118, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 119, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 121, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 122, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 123, 0);
            BuildingManager.Instance.QuickBuild(4, 122, 118, 0);
            BuildingManager.Instance.QuickBuild(9, 130, 120, 0);
            BuildingManager.Instance.QuickBuild(2, 130, 113, 0);
            BuildingManager.Instance.QuickBuild(2, 132, 113, 0);
            BuildingManager.Instance.QuickBuild(11, 115, 109, 0);
            BuildingManager.Instance.QuickBuild(11, 115, 103, 0);
            BuildingManager.Instance.QuickBuild(14, 129, 107, 0);
            BuildingManager.Instance.QuickBuild(14, 135, 105, 1);
            BuildingManager.Instance.QuickBuild(14, 128, 105, 0);
            BuildingManager.Instance.QuickBuild(14, 131, 105, 1);
            BuildingManager.Instance.QuickBuild(2, 130, 111, 0);
            BuildingManager.Instance.QuickBuild(2, 132, 111, 0);
            BuildingManager.Instance.QuickBuild(2, 134, 113, 0);
            BuildingManager.Instance.QuickBuild(2, 134, 111, 0);
            BuildingManager.Instance.QuickBuild(2, 136, 111, 0);
            BuildingManager.Instance.QuickBuild(2, 136, 113, 0);
            BuildingManager.Instance.QuickBuild(9, 135, 121, 0);
            BuildingManager.Instance.QuickBuild(9, 140, 121, 0);
            BuildingManager.Instance.QuickBuild(10, 120, 96, 0);
            BuildingManager.Instance.QuickBuild(12, 135, 75, 0);
            BuildingManager.Instance.QuickBuild(13, 142, 94, 0);
            BuildingManager.Instance.QuickBuild(9, 118, 89, 0);
            BuildingManager.Instance.QuickBuild(9, 123, 89, 0);
            BuildingManager.Instance.QuickBuild(10, 109, 96, 0);
            BuildingManager.Instance.QuickBuild(10, 101, 97, 0);
            BuildingManager.Instance.QuickBuild(12, 130, 88, 0);
            BuildingManager.Instance.QuickBuild(10, 103, 90, 0);
            BuildingManager.Instance.QuickBuild(28, 106, 84, 0);
            BuildingManager.Instance.QuickBuild(8, 112, 82, 0);
            BuildingManager.Instance.QuickBuild(8, 118, 82, 0);
            BuildingManager.Instance.QuickBuild(9, 111, 89, 0);
            BuildingManager.Instance.QuickBuild(9, 104, 87, 0);
            BuildingManager.Instance.QuickBuild(8, 101, 83, 0);
            BuildingManager.Instance.QuickBuild(8, 95, 80, 0);
            BuildingManager.Instance.QuickBuild(9, 125, 110, 0);
            BuildingManager.Instance.QuickBuild(9, 124, 83, 0);
        }
    }
}