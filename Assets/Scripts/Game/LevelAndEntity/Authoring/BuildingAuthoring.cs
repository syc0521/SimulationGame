using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class BuildingAuthoring : MonoBehaviour
    {
        public int type;
        public int maxPeople;
        public GameObject meshRoot;
        public int maxLevel;
        class BuildingBaker : Baker<BuildingAuthoring>
        {
            public override void Bake(BuildingAuthoring authoring)
            {
                AddComponent(new Building
                {
                    type = authoring.type,
                    maxPeople = authoring.maxPeople,
                    maxLevel = authoring.maxLevel,
                    meshRoot = GetEntity(authoring.meshRoot)
                });
            }
        }
    }
}