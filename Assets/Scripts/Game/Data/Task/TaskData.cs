using System;
using Game.Core;

namespace Game.Data
{
    [Serializable]
    public struct TaskData
    {
        public uint taskID;
        public int previousID;
        public string name;
        public string content;
        public TaskType taskType;
        public uint targetID;
        public int targetNum;
        public RewardData rewardData;

        public TaskState TaskState => Managers.Get<ISaveDataManager>().GetTaskState(taskID);
    }
}