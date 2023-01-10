using System.Collections.Generic;
using Game.Core;

namespace Game.Data
{
    public interface ISaveDataManager : IManager
    {
        void SaveData();
        Dictionary<uint, BuildingData> GetBuildings();
        TaskState GetTaskState(int id);
        void ActivateTask(int id);
        bool ChangeTaskState(int id, TaskState state);
        bool ChangeTaskNum(int id, int num);
        int GetTaskNum(int id);
        Dictionary<int, PlayerTaskData> GetCurrentTasks();
    }
}