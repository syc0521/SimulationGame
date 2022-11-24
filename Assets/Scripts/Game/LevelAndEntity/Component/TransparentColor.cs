using Unity.Entities;
using Unity.Rendering;

namespace Game.LevelAndEntity.Component
{
    [MaterialProperty("_IsTransparency")]
    public struct TransparentColor : IComponentData
    {
        public float isTransparency;
    }
}