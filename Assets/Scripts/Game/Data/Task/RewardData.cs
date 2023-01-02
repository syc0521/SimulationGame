using System;

namespace Game.Data
{
    public enum RewardType
    {
        Currency = 0,
        Building = 1,
        Item = 2,
    }
    
    [Serializable]
    public struct RewardData
    {
        public RewardType type;
        public uint rewardID;
        public int amount;
    }
}