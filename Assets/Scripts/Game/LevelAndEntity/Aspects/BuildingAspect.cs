using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.LevelAndEntity.Aspects
{
    readonly partial struct BuildingAspect : IAspect
    {
        private readonly TransformAspect transform;
        private readonly RefRO<Building> building;

        public int People => building.ValueRO.type + 1;

        public float3 Position
        {
            get => transform.Position;
            set => transform.Position = value;
        }

    }
}
