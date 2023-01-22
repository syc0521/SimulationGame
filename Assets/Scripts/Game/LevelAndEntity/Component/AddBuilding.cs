using Unity.Entities;
using Unity.Mathematics;

namespace Game.LevelAndEntity.Component
{
    public struct AddBuilding : IComponentData
    {
        public uint id;
        public float3 spawnPos;
        public int spawnType;
        public int rotation;
    }
}