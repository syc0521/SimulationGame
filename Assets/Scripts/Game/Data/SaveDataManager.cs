using System.Collections.Generic;
using Game.Core;

namespace Game.Data
{
    public class SaveDataManager : ManagerBase, ISaveDataManager
    {
        private PlayerData _playerData;
        
        public void SaveData()
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<uint, BuildingData> GetBuildings()
        {
            return new Dictionary<uint, BuildingData>();
        }
    }
}