using Game.LevelAndEntity.Component;
using Unity.Collections;
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
        public float cd;
        public int envValue;
        public int evaluateScore;
        class BuildingBaker : Baker<BuildingAuthoring>
        {
            public override void Bake(BuildingAuthoring authoring)
            {
                AddComponent(new Building
                {
                    type = authoring.type,
                    maxPeople = authoring.maxPeople,
                    maxLevel = authoring.maxLevel,
                    meshRoot = GetEntity(authoring.meshRoot),
                    cd = authoring.cd,
                    envValue = authoring.envValue,
                    evaluateScore = authoring.evaluateScore,
                });
            }
        }
    }
}