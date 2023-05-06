using Game.Core;
using Game.Data.Shop;

namespace Game.Data.Event.Shop
{
    public struct BuySuccessEvent : IEvent
    {
        public RewardData RewardData;
    }
}