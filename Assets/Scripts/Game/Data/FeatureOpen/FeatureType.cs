using System;

namespace Game.Data.FeatureOpen
{
    public enum FeatureType
    {
        // 功能
        Move = 1,
        Rotate = 2,
        Backpack = 3,
        Achievement = 4,
        [Obsolete]
        Destroy = 5,
        Upgrade = 6,
        Government = 7,
        
        // 控制任务章节
        Chapter1 = 101,
        Chapter2 = 102,
        Chapter3 = 103,
    }
}