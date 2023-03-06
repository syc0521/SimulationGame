using Game.Core;

namespace Game.Data.Event.Common
{
    public struct LoadingEvent : IEvent
    {
        public float progress;
        public string text;
    }
}