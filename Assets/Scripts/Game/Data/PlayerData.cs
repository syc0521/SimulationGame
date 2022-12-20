using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public struct PlayerData
    {
        public int money;
        public Dictionary<uint, BuildingData> buildings;
    }

    [Serializable]
    public struct BuildingData
    {
        public int type;
        public int level;
        public Vector2 position;
        public int rotation; // 0 1 2 3
    }
}