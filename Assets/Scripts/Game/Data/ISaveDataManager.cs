using System.Collections.Generic;
using Game.Core;

namespace Game.Data
{
    public interface ISaveDataManager : IManager
    {
        void SaveData();
        Dictionary<uint, BuildingData> GetBuildings();
        void GetPlayerTasks(ref Dictionary<int, PlayerTaskData> taskData);
    }
}