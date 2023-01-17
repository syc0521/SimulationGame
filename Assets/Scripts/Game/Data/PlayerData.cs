using System;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    [MessagePackObject]
    public class PlayerData
    {
        [Key(0)]
        public int money;
        [Key(1)]
        public Dictionary<uint, BuildingData> buildings;
        [Key(2)] 
        public Dictionary<int, PlayerTaskData> tasks;
    }

    [Serializable]
    [MessagePackObject]
    public struct BuildingData
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
}