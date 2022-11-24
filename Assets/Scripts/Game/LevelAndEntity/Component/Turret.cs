using Unity.Entities;

namespace Game.LevelAndEntity.Component
{
    public struct Turret : IComponentData
    {
        public Entity CannonBallSpawn;
        public Entity CannonBallPrefab;
    }
}