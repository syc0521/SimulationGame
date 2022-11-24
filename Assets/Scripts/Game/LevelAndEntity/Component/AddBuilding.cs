using Unity.Entities;
using Unity.Mathematics;

namespace Game.LevelAndEntity.Component
{
    public struct AddBuilding : IComponentData
    {
        public float3 spawnPos;
        public int spawnType;
    }
}