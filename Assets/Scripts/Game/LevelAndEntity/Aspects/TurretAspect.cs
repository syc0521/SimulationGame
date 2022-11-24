using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Game.LevelAndEntity.Aspects
{
    readonly partial struct TurretAspect : IAspect
    {
        private readonly RefRO<Turret> turret;
        private readonly RefRO<URPMaterialPropertyBaseColor> baseColor;
        public Entity CannonBallSpawn => turret.ValueRO.CannonBallSpawn;
        public Entity CannonBallPrefab => turret.ValueRO.CannonBallPrefab;
        public float4 Color => baseColor.ValueRO.Value;
    }
}