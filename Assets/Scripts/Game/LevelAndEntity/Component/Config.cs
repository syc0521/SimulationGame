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
        public bool dataLoaded;
        public int people;
        public float envRate;
        public float happiness;
        public float buildingRate;
    }
}