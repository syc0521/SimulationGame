using System;
using System.Collections.Generic;
using MessagePack;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    [MessagePackObject]
    public struct PlayerData
    {
        [Key(0)]
        public int money;
        [Key(1)]
        public Dictionary<uint, BuildingData> buildings;
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
}