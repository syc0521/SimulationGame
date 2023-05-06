using Game.Core;

namespace Game.Data.Event
{
    public enum HUDType
    {
        All = 0,
        Build = 1,
    }
    
    public struct ShowHUDEvent : IEvent
    {
        public HUDType HUDType;
    }
}