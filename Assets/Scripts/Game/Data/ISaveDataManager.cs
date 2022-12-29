using System.Collections.Generic;
using Game.Core;

namespace Game.Data
{
    public interface ISaveDataManager : IManager
    {
        void SaveData();
        void LoadData();
        Dictionary<uint, BuildingData> GetBuildings();
    }
}