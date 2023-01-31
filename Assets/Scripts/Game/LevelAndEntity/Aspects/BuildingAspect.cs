using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.LevelAndEntity.Aspects
{
    public readonly partial struct BuildingAspect : IAspect
    {
        private readonly TransformAspect transform;
        private readonly RefRO<Building> building;
        private readonly RefRW<LevelObject> levelObject;
        private readonly RefRW<Timer> timer;
        public readonly Entity self;

        public int People => building.ValueRO.type + 1;
        public int BuildingType => building.ValueRO.type;
        public uint ID => levelObject.ValueRO.id;
        public int Level => levelObject.ValueRO.level; // 等级

        public float CurrentTime
        {
            get => timer.ValueRW.currentTime;
            set => timer.ValueRW.currentTime = value;
        }

        public float3 Position
        {
            get => transform.Position;
            set => transform.Position = value;
        }

        public quaternion LocalRotation
        {
            get => transform.LocalRotation;
            set => transform.LocalRotation = value;
        }

        public float3 SpawnPos
        {
            get => levelObject.ValueRO.spawnPos;
            set => levelObject.ValueRW.spawnPos = value;
        }

    }
}
