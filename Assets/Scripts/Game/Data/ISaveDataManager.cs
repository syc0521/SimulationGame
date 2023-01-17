using System.Collections.Generic;
using Game.Core;

namespace Game.Data
{
    public interface ISaveDataManager : IManager
    {
        void SaveData();
        Dictionary<uint, BuildingData> GetBuildings();
        Dictionary<int, PlayerTaskData> GetPlayerTasks();
    }
}