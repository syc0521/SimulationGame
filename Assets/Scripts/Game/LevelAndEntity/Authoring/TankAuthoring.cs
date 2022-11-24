using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class TankAuthoring : MonoBehaviour
    {
        
    }

    public class TankBaker : Baker<TankAuthoring>
    {
        public override void Bake(TankAuthoring authoring)
        {
            AddComponent<Tank>();
        }
    }
}