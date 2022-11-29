using Unity.Entities;
using Unity.Mathematics;

namespace Game.LevelAndEntity.Component
{
    public struct CannonBall : IComponentData
    {
        public float3 speed;
    }
}