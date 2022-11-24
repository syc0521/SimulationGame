using System.Collections.Generic;
using Game.Data.ScriptableObject;
using Game.LevelAndEntity.Component;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

namespace Game.LevelAndEntity.Authoring
{
    public class ConfigAuthoring : MonoBehaviour
    {
        public List<GameObject> list;
    }

    public class ConfigBaker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            AddComponent(new Config());
            var buffer = AddBuffer<PrefabSpawnerBufferElement>();
            foreach (var item in authoring.list)
            {
                buffer.Add(new PrefabSpawnerBufferElement { prefab = GetEntity(item) });
            }
        }
    }
}