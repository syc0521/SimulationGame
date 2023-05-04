using Game.Core;

namespace Game.Data.Event
{
    public struct OpenBuildingInfoEvent : IEvent
    {
        public int id;
        public bool isStatic;
    }

    public struct OpenBuildingDetailEvent : IEvent
    {
        public int id;
    }
}