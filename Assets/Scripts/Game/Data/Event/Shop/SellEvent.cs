using Game.Core;

namespace Game.Data.Event.Shop
{
    public struct SellEvent : IEvent
    {
        public RewardData RewardData;
    }
}