using System.Collections.Generic;
using Game.Core;

namespace Game.Data
{
    public interface ISaveDataManager : IManager
    {
        void SaveData();
        void LoadData();
        Dictionary<uint, BuildingData> GetBuildings();
        TaskState GetTaskState(uint id);
        void ActivateTask(uint id);
        bool ChangeTaskState(uint id, TaskState state);
        bool ChangeTaskNum(uint id, int num);
        int GetTaskNum(uint id);
    }
}