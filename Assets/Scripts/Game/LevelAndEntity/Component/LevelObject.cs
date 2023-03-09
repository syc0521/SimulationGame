using Unity.Entities;
using Unity.Mathematics;

namespace Game.LevelAndEntity.Component
{
    public struct LevelObject : IComponentData
    {
        public uint id;
        public int level;
        public float3 spawnPos;
        public bool isStatic;
    }
}