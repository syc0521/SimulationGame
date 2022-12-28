using Unity.Entities;

namespace Game.LevelAndEntity.Component
{
    public struct Timer : IComponentData
    {
        public float cd;
        public float currentTime;
    }
}