using Game.Core;
using Game.Data.FeatureOpen;

namespace Game.Data.Event.FeatureOpen
{
    public struct UnlockFeatureEvent : IEvent
    {
        public FeatureType type;
    }
}