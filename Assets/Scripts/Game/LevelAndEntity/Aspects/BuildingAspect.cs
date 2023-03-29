using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.LevelAndEntity.Aspects
{
    public readonly partial struct BuildingAspect : IAspect
    {
        private readonly RefRW<LocalTransform> transform;
        private readonly RefRO<Building> building;
        private readonly RefRW<LevelObject> levelObject;
        private readonly RefRW<Timer> timer;
        public readonly Entity self;

        public int People => (int)(MaxPeople * ((float)Level / MaxLevel));
        public int BuildingType => building.ValueRO.type;
        public uint ID => levelObject.ValueRO.id;
        public int Level => levelObject.ValueRO.level; // 等级
        public int MaxLevel => building.ValueRO.maxLevel;
        public int MaxPeople => building.ValueRO.maxPeople;
        public float CurrentCD => building.ValueRO.cd * (1 - (Level - 1) / (MaxLevel * 2 + 1.0f));
        public int EnvScore => building.ValueRO.envValue;
        public int EvaluateScore => (int)(building.ValueRO.evaluateScore * math.sqrt(Level));

        public float CurrentTime
        {
            get => timer.ValueRW.currentTime;
            set => timer.ValueRW.currentTime = value;
        }

        public float3 Position
        {
            get => transform.ValueRO.Position;
            set => transform.ValueRW.Position = value;
        }

        public quaternion LocalRotation
        {
            get => transform.ValueRO.Rotation;
            set => transform.ValueRW.Rotation = value;
        }

        public float3 SpawnPos
        {
            get => levelObject.ValueRO.spawnPos;
            set => levelObject.ValueRW.spawnPos = value;
        }
        
    }
}
