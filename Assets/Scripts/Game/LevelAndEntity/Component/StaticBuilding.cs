using Unity.Entities;

namespace Game.LevelAndEntity.Component
{
    public struct StaticBuilding : IComponentData
    {
        public int id;
        public int row, col;
    }
}