using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class TurretAuthoring : MonoBehaviour
    {
        public GameObject CannonBallPrefab;
        public Transform CannonBallSpawn;
    }

    public class TurretBaker : Baker<TurretAuthoring>
    {
        public override void Bake(TurretAuthoring authoring)
        {
            AddComponent(new Turret
            {
                CannonBallPrefab = GetEntity(authoring.CannonBallPrefab),
                CannonBallSpawn = GetEntity(authoring.CannonBallSpawn)
            });
        
            AddComponent<Shooting>();
        }
    }
}