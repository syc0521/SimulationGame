using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class StaticBuildingAuthoring : MonoBehaviour
    {
        public int id;
        public int row, col;

        class StaticBuildingBaker : Baker<StaticBuildingAuthoring>
        {
            public override void Bake(StaticBuildingAuthoring authoring)
            {
                AddComponent(new LevelObject
                {
                    id = (uint)authoring.id + 10000,
                    isStatic = true,
                    level = 1,
                });
                AddComponent(new StaticBuilding
                {
                    id = authoring.id,
                    col = authoring.col,
                    row = authoring.row,
                });
            }
        }
    }
}