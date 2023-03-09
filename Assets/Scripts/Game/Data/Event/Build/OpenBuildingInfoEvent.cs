using Game.Core;

namespace Game.Data.Event
{
    public struct OpenBuildingInfoEvent : IEvent
    {
        public int id;
        public bool isStatic;
    }
}