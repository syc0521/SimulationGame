using System.Collections.Generic;
using Game.Core;

namespace Game.Data
{
    public interface ISaveDataManager : IManager
    {
        void SaveData();
        void GetBuildings(ref Dictionary<uint, BuildingData> buildings);
        void GetPlayerTasks(ref Dictionary<int, PlayerTaskData> taskData);
    }
}