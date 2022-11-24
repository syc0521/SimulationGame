using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.LevelAndEntity.Aspects
{
    readonly partial struct BuildingAspect : IAspect
    {
        private readonly TransformAspect transform;

        public float3 Position
        {
            get => transform.Position;
            set => transform.Position = value;
        }

    }
}
