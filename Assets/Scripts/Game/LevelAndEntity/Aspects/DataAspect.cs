using Game.LevelAndEntity.Component;
using Unity.Entities;

namespace Game.LevelAndEntity.Aspects
{
    public readonly partial struct DataAspect : IAspect
    {
        public readonly RefRW<Config> config;
    }
}