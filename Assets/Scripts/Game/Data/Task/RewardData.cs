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
        public int rewardID;
        public int itemID;
        public int amount;

        public RewardData(TableData.RewardData data)
        {
            rewardID = data.Rewardid;
            type = (RewardType)data.Rewardtype;
            itemID = data.Itemid;
            amount = data.Count;
        }
    }
}