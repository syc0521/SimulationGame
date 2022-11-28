using Game.Data.ScriptableObject;
using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class ConfigAuthoring : MonoBehaviour
    {
        public BuildingCollection list;
    }

    public class ConfigBaker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            AddComponent(new Config());
            var buffer = AddBuffer<PrefabSpawnerBufferElement>();
            foreach (var item in authoring.list.buildings)
            {
                buffer.Add(new PrefabSpawnerBufferElement { prefab = GetEntity(item) });
            }
        }
    }
}