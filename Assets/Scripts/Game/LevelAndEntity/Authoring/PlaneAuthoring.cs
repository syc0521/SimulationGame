using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class PlaneAuthoring : MonoBehaviour
    {

        class PlaneBaker : Baker<PlaneAuthoring>
        {
            public override void Bake(PlaneAuthoring authoring)
            {
                AddComponent(new BuildingPlane());
            }
        }
    }
}