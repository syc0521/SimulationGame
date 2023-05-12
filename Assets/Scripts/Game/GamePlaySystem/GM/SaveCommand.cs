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

    [GMAttr(type = "存档", name = "2号城市", priority = 4)]
    public class QuickBuild2Command : ICommand
    {
        public void Run()
        {
            BuildingManager.Instance.QuickBuild(11, 143, 128, 0);
            BuildingManager.Instance.QuickBuild(4, 145, 121, 0);
            BuildingManager.Instance.QuickBuild(4, 145, 122, 0);
            BuildingManager.Instance.QuickBuild(4, 145, 123, 0);
            BuildingManager.Instance.QuickBuild(4, 145, 124, 0);
            BuildingManager.Instance.QuickBuild(4, 145, 125, 0);
            BuildingManager.Instance.QuickBuild(4, 145, 126, 0);
            BuildingManager.Instance.QuickBuild(4, 145, 127, 0);
            BuildingManager.Instance.QuickBuild(4, 125, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 126, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 127, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 128, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 129, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 130, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 131, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 132, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 133, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 134, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 135, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 136, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 137, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 138, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 139, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 140, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 141, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 142, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 143, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 144, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 145, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 121, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 122, 0);
            BuildingManager.Instance.QuickBuild(4, 124, 123, 0);
            BuildingManager.Instance.QuickBuild(2, 124, 111, 1);
            BuildingManager.Instance.QuickBuild(2, 127, 111, 0);
            BuildingManager.Instance.QuickBuild(9, 132, 110, 0);
            BuildingManager.Instance.QuickBuild(9, 132, 101, 0);
            BuildingManager.Instance.QuickBuild(11, 142, 111, 0);
            BuildingManager.Instance.QuickBuild(11, 142, 102, 0);
            BuildingManager.Instance.QuickBuild(11, 142, 94, 0);
            BuildingManager.Instance.QuickBuild(14, 130, 108, 1);
            BuildingManager.Instance.QuickBuild(14, 131, 107, 0);
            BuildingManager.Instance.QuickBuild(14, 135, 107, 0);
            BuildingManager.Instance.QuickBuild(14, 133, 108, 0);
            BuildingManager.Instance.QuickBuild(2, 124, 108, 0);
            BuildingManager.Instance.QuickBuild(2, 127, 108, 0);
            BuildingManager.Instance.QuickBuild(2, 124, 102, 0);
            BuildingManager.Instance.QuickBuild(2, 127, 102, 0);
            BuildingManager.Instance.QuickBuild(2, 124, 99, 0);
            BuildingManager.Instance.QuickBuild(2, 127, 99, 0);
            BuildingManager.Instance.QuickBuild(14, 131, 98, 0);
            BuildingManager.Instance.QuickBuild(14, 133, 98, 0);
            BuildingManager.Instance.QuickBuild(14, 130, 99, 1);
            BuildingManager.Instance.QuickBuild(14, 136, 99, 1);
            BuildingManager.Instance.QuickBuild(14, 135, 98, 0);
            BuildingManager.Instance.QuickBuild(9, 132, 94, 0);
            BuildingManager.Instance.QuickBuild(2, 124, 95, 0);
            BuildingManager.Instance.QuickBuild(2, 127, 95, 0);
            BuildingManager.Instance.QuickBuild(2, 124, 92, 0);
            BuildingManager.Instance.QuickBuild(2, 127, 92, 0);
            BuildingManager.Instance.QuickBuild(14, 130, 92, 1);
            BuildingManager.Instance.QuickBuild(14, 131, 91, 0);
            BuildingManager.Instance.QuickBuild(14, 133, 91, 0);
            BuildingManager.Instance.QuickBuild(14, 135, 91, 0);
            BuildingManager.Instance.QuickBuild(14, 137, 92, 3);
            BuildingManager.Instance.QuickBuild(10, 138, 139, 0);
            BuildingManager.Instance.QuickBuild(12, 108, 141, 0);
            BuildingManager.Instance.QuickBuild(13, 101, 131, 0);
            BuildingManager.Instance.QuickBuild(9, 134, 125, 0);
            BuildingManager.Instance.QuickBuild(9, 134, 131, 0);
            BuildingManager.Instance.QuickBuild(10, 125, 144, 0);
            BuildingManager.Instance.QuickBuild(10, 100, 143, 0);
            BuildingManager.Instance.QuickBuild(20, 120, 145, 0);
            BuildingManager.Instance.QuickBuild(30, 94, 119, 0);
            BuildingManager.Instance.QuickBuild(10, 132, 142, 0);
            BuildingManager.Instance.QuickBuild(12, 108, 127, 0);
            BuildingManager.Instance.QuickBuild(9, 118, 109, 0);
            BuildingManager.Instance.QuickBuild(9, 118, 100, 0);
            BuildingManager.Instance.QuickBuild(9, 118, 94, 0);
            BuildingManager.Instance.QuickBuild(8, 110, 109, 0);
            BuildingManager.Instance.QuickBuild(8, 110, 100, 0);
            BuildingManager.Instance.QuickBuild(8, 110, 93, 0);
            BuildingManager.Instance.QuickBuild(9, 118, 89, 0);
            BuildingManager.Instance.QuickBuild(9, 118, 85, 0);
            BuildingManager.Instance.QuickBuild(9, 118, 81, 0);
            BuildingManager.Instance.QuickBuild(8, 110, 87, 0);
            BuildingManager.Instance.QuickBuild(8, 110, 82, 0);
            BuildingManager.Instance.QuickBuild(28, 91, 133, 0);
            BuildingManager.Instance.QuickBuild(24, 88, 124, 0);
            BuildingManager.Instance.QuickBuild(25, 87, 130, 1);
            BuildingManager.Instance.QuickBuild(17, 94, 114, 0);
            BuildingManager.Instance.QuickBuild(2, 124, 88, 0);
            BuildingManager.Instance.QuickBuild(2, 127, 88, 0);
            BuildingManager.Instance.QuickBuild(2, 130, 88, 0);
            BuildingManager.Instance.QuickBuild(2, 133, 88, 0);
            BuildingManager.Instance.QuickBuild(2, 136, 88, 0);
            BuildingManager.Instance.QuickBuild(2, 139, 88, 0);
            BuildingManager.Instance.QuickBuild(2, 142, 88, 0);
            BuildingManager.Instance.QuickBuild(2, 124, 84, 0);
            BuildingManager.Instance.QuickBuild(2, 124, 80, 0);
            BuildingManager.Instance.QuickBuild(2, 127, 84, 1);
            BuildingManager.Instance.QuickBuild(2, 127, 80, 0);
            BuildingManager.Instance.QuickBuild(2, 130, 84, 0);
            BuildingManager.Instance.QuickBuild(2, 133, 84, 0);
            BuildingManager.Instance.QuickBuild(2, 136, 84, 0);
            BuildingManager.Instance.QuickBuild(2, 139, 84, 0);
            BuildingManager.Instance.QuickBuild(2, 142, 84, 0);
            BuildingManager.Instance.QuickBuild(2, 130, 80, 0);
            BuildingManager.Instance.QuickBuild(2, 133, 80, 0);
            BuildingManager.Instance.QuickBuild(2, 136, 80, 0);
            BuildingManager.Instance.QuickBuild(2, 139, 80, 0);
            BuildingManager.Instance.QuickBuild(2, 142, 80, 0);
            BuildingManager.Instance.QuickBuild(3, 91, 126, 0);
            BuildingManager.Instance.QuickBuild(16, 101, 98, 0);
            BuildingManager.Instance.QuickBuild(10, 153, 129, 1);
            BuildingManager.Instance.QuickBuild(10, 152, 109, 1);
            BuildingManager.Instance.QuickBuild(10, 152, 97, 1);
            BuildingManager.Instance.QuickBuild(10, 151, 88, 1);
            BuildingManager.Instance.QuickBuild(4, 117, 105, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 106, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 107, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 108, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 109, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 110, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 111, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 112, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 113, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 114, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 115, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 116, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 117, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 118, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 95, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 96, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 97, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 98, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 99, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 100, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 101, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 102, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 103, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 104, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 81, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 82, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 83, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 84, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 85, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 86, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 87, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 88, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 89, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 90, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 91, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 92, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 93, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 94, 0);
            BuildingManager.Instance.QuickBuild(4, 115, 80, 0);
            BuildingManager.Instance.QuickBuild(4, 116, 80, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 80, 0);
            BuildingManager.Instance.QuickBuild(4, 100, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 101, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 102, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 103, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 104, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 105, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 106, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 107, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 108, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 109, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 110, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 111, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 112, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 113, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 80, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 81, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 82, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 83, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 84, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 85, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 86, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 87, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 88, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 89, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 90, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 91, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 92, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 93, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 94, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 95, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 96, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 97, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 98, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 99, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 100, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 101, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 102, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 103, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 104, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 105, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 106, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 107, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 108, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 109, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 110, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 111, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 112, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 113, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 114, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 115, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 116, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 117, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 118, 0);
            BuildingManager.Instance.QuickBuild(4, 114, 119, 0);
            BuildingManager.Instance.QuickBuild(4, 118, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 119, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 120, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 121, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 122, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 123, 120, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 119, 0);
            BuildingManager.Instance.QuickBuild(4, 117, 120, 0);
            BuildingManager.Instance.QuickBuild(6, 104, 109, 0);
            BuildingManager.Instance.QuickBuild(6, 104, 101, 0);
            BuildingManager.Instance.QuickBuild(6, 104, 94, 0);
            BuildingManager.Instance.QuickBuild(6, 104, 89, 2);
            BuildingManager.Instance.QuickBuild(6, 104, 83, 2);
            BuildingManager.Instance.QuickBuild(12, 123, 64, 0);
            BuildingManager.Instance.QuickBuild(12, 134, 64, 0);
            BuildingManager.Instance.QuickBuild(13, 113, 68, 2);
            BuildingManager.Instance.QuickBuild(24, 88, 93, 0);
            BuildingManager.Instance.QuickBuild(17, 92, 85, 0);
            BuildingManager.Instance.QuickBuild(15, 99, 84, 0);
            BuildingManager.Instance.QuickBuild(3, 93, 123, 1);
            BuildingManager.Instance.QuickBuild(21, 98, 108, 0);
            BuildingManager.Instance.QuickBuild(21, 89, 89, 0);
            BuildingManager.Instance.QuickBuild(10, 147, 142, 2);
            BuildingManager.Instance.QuickBuild(30, 94, 132, 0);
            BuildingManager.Instance.QuickBuild(30, 122, 115, 0);
            BuildingManager.Instance.QuickBuild(29, 95, 90, 0);
            BuildingManager.Instance.QuickBuild(26, 135, 58, 0);
            BuildingManager.Instance.QuickBuild(27, 103, 69, 0);
            BuildingManager.Instance.QuickBuild(31, 74, 96, 0);
            BuildingManager.Instance.QuickBuild(7, 115, 121, 0);
            BuildingManager.Instance.QuickBuild(30, 94, 72, 0);
            BuildingManager.Instance.QuickBuild(30, 113, 61, 0);
            BuildingManager.Instance.QuickBuild(30, 104, 62, 0);
            BuildingManager.Instance.QuickBuild(30, 95, 63, 0);
            BuildingManager.Instance.QuickBuild(27, 84, 67, 0);
        }
    }
    
}