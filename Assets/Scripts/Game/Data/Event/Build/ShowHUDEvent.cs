using Game.Core;

namespace Game.Data.Event
{
    public enum HUDType
    {
        All = 0,
        Build = 1,
        Detail = 2,
    }
    
    public struct ShowHUDEvent : IEvent
    {
        public HUDType HUDType;
    }
}