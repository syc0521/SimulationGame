﻿using Game.Core;
using Unity.Mathematics;

namespace Game.Data.Event
{
    public struct StaticBuildingIntlEvent : IEvent
    {
        public float3 pos;
        public int row, col;
        public int id;
    }
}