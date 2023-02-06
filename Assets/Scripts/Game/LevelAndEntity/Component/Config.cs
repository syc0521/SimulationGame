using Game.Data.ScriptableObject;
using Unity.Collections;
using Unity.Entities;

namespace Game.LevelAndEntity.Component
{
    public struct PrefabSpawnerBufferElement : IBufferElementData
    {
        public Entity prefab;
    }
    
    public struct Config : IComponentData
    {
        public bool dataChanged;
        public int money;
        public int people;
    }
}