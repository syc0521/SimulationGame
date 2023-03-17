using System;
using System.Collections.Generic;
using Game.Data.FeatureOpen;
using MessagePack;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    [MessagePackObject]
    public class PlayerData
    {
        [Key(1)]
        public Dictionary<uint, BuildingData> buildings;
        [Key(2)] 
        public Dictionary<int, PlayerTaskData> tasks;
        [Key(3)] 
        public Dictionary<int, int> backpack;
        [Key(4)]
        public Dictionary<int, int> currency;
        [Key(5)]
        public SettingData settingData;
        [Key(6)] 
        public HashSet<int> unlockedBuildings;
        [Key(7)] 
        public DateTime lastLoginTime;
        [Key(8)] 
        public HashSet<FeatureType> unlockedFeatures;
        [Key(9)] 
        public Dictionary<int, PlayerAchievementData> achievementData;
    }

    [Serializable]
    [MessagePackObject]
    public class BuildingData
    {
        [Key(0)]
        public int type;
        [Key(1)]
        public int level;
        [Key(2)]
        public Vector2 position;
        [Key(3)]
        public int rotation; // 0 1 2 3
    }

    [Serializable]
    [MessagePackObject]
    public class PlayerTaskData
    {
        [Key(0)]
        public TaskState state;
        [Key(1)]
        public int[] currentNum;
    }

    [Serializable]
    [MessagePackObject]
    public class SettingData
    {
        [Key(0)]
        public float bgmVolume;
        [Key(1)]
        public float soundVolume;
    }

    [Serializable]
    [MessagePackObject]
    public class PlayerAchievementData
    {
        [Key(0)] 
        public bool complete;
        [Key(1)]
        public int progress;
    }
}