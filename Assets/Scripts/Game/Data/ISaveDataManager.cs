using System.Collections.Generic;
using Game.Core;

namespace Game.Data
{
    public interface ISaveDataManager : IManager
    {
        void SaveData();
        void ResetSaveData();
        void GetBuildings(ref Dictionary<uint, BuildingData> buildings);
        void GetPlayerTasks(ref Dictionary<int, PlayerTaskData> taskData);
        void GetBackpack(ref Dictionary<int, int> backpack);
        void GetCurrency(ref Dictionary<int, int> currency);
        void GetUnlockedBuildings(ref HashSet<int> unlockedBuildings);
    }
}