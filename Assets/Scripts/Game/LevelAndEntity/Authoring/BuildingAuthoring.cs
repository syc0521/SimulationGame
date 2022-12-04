using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class BuildingAuthoring : MonoBehaviour
    {
        public int type;
        public GameObject meshRoot;
        class BuildingBaker : Baker<BuildingAuthoring>
        {
            public override void Bake(BuildingAuthoring authoring)
            {
                AddComponent(new Building
                {
                    cd = 10,
                    type = authoring.type,
                    meshRoot = GetEntity(authoring.meshRoot)
                });
            }
        }
    }
}