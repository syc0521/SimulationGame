using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.LevelAndEntity.Aspects
{
    readonly partial struct CannonBallAspect : IAspect
    {
        public readonly Entity self;
        private readonly TransformAspect transform;
        private readonly RefRW<CannonBall> cannonBall;

        public float3 Position
        {
            get => transform.Position;
            set => transform.Position = value;
        }

        public float3 Speed
        {
            get => cannonBall.ValueRO.speed;
            set => cannonBall.ValueRW.speed = value;
        }
    }
}