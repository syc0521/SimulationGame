using Game.Core;
using Game.Data;
using Game.Data.ScriptableObject;
using Game.LevelAndEntity.Component;
using Game.LevelAndEntity.ResLoader;
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
            foreach (var item in ConfigTable.Instance.GetBuildingData())
            {
                if (Managers.Get<IResLoader>().LoadRes(item.Resourcepath, out var obj))
                {
                    buffer.Add(new PrefabSpawnerBufferElement { prefab = GetEntity(obj) });
                }
            }
            foreach (var item in authoring.list.buildings)
            {
                buffer.Add(new PrefabSpawnerBufferElement { prefab = GetEntity(item) });
            }
        }
    }
}