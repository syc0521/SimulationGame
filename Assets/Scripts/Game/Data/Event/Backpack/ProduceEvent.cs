using Game.Core;
using Game.Data.Common;

namespace Game.Data.Event.Backpack
{
    public struct ProduceEvent : IEvent
    {
        public ProduceType produceType;
        public int produceID;
        public int count;
    }
}