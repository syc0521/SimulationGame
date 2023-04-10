using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.LevelAndEntity.Aspects
{
    public readonly partial struct StaticBuildingAspect : IAspect
    {
        private readonly RefRW<LocalTransform> transform;
        public readonly Entity self;
        private readonly RefRO<StaticBuilding> staticBuilding;
        private readonly RefRW<LevelObject> levelObject;

        public uint ID => levelObject.ValueRO.id;
        public int Row => staticBuilding.ValueRO.row;
        public int Col => staticBuilding.ValueRO.col;

        public float3 Position
        {
            get => transform.ValueRO.Position;
            set => transform.ValueRW.Position = value;
        }

        public int Level
        {
            get => levelObject.ValueRO.level;
            set => levelObject.ValueRW.level = value;
        }
    }
}