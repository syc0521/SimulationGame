using Game.LevelAndEntity.Component;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class CannonBallAuthoring : MonoBehaviour
    {
        
    }

    public class CannonBallBaker : Baker<CannonBallAuthoring>
    {
        public override void Bake(CannonBallAuthoring authoring)
        {
            AddComponent<CannonBall>();
        }
    }
}