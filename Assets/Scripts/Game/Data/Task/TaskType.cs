namespace Game.Data
{
    public enum TaskType
    {
        AddBuilding = 0,
        MoveBuilding = 1,
        GetCurrency = 2,
        GetBagItem = 3,
        RotateBuilding = 4,
        UpgradeBuilding = 5,
        GetEvaluateScore = 6,
        People = 7,
        BuyItem = 8,
        BuyDailyItem = 9,
        SellItem = 10,
        SellToGetCurrency = 11,
    }

    public enum TaskState
    {
        NotAccept = 0,
        Accepted = 1,
        Finished = 2,
        Rewarded = 3,
        Error = -1,
    }
}