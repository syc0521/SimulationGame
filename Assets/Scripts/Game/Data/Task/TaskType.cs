namespace Game.Data
{
    public enum TaskType
    {
        AddBuilding = 0,
        MoveBuilding = 1,
        GetCurrency = 2,
        GetBagItem = 3,
        RotateBuilding = 4,
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