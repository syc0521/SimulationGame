using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;

namespace Game.LevelAndEntity.Component
{
    public struct Building : IComponentData
    {
        public float cd;
    }
}