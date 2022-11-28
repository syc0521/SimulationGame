using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class BuildingAuthoring : MonoBehaviour
    {
        public int type;
        class BuildingBaker : Baker<BuildingAuthoring>
        {
            public override void Bake(BuildingAuthoring authoring)
            {
                AddComponent(new Building
                {
                    cd = 10,
                    type = authoring.type,
                });
            }
        }
    }
}