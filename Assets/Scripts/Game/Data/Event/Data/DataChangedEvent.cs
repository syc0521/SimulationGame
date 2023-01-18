using Game.Core;

namespace Game.Data.Event
{
    public struct DataChangedEvent : IEvent
    {
        public GameData gameData;
    }
}