using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class StaticBuildingAuthoring : MonoBehaviour
    {
        public int id;

        class StaticBuildingBaker : Baker<StaticBuildingAuthoring>
        {
            public override void Bake(StaticBuildingAuthoring authoring)
            {
                AddComponent(new LevelObject
                {
                    id = (uint)authoring.id,
                });
                AddComponent(new StaticBuilding
                {
                    id = authoring.id
                });
            }
        }
    }
}