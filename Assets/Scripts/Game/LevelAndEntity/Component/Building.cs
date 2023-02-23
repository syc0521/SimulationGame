using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;

namespace Game.LevelAndEntity.Component
{
    public struct Building : IComponentData
    {
        public int type;
        public int maxPeople;
        public int maxLevel;
        public NativeArray<int> cd;
        public int envValue;
        public int evaluateScore;
        public Entity meshRoot;
    }
}