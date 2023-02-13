using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.LevelAndEntity.Aspects
{
    public readonly partial struct StaticBuildingAspect : IAspect
    {
        private readonly TransformAspect transform;
        public readonly Entity self;
        private readonly RefRO<StaticBuilding> staticBuilding;

        public int ID => staticBuilding.ValueRO.id;
        public int Row => staticBuilding.ValueRO.row;
        public int Col => staticBuilding.ValueRO.col;
        
        public float3 Position
        {
            get => transform.WorldPosition;
            set => transform.WorldPosition = value;
        }
    }
}