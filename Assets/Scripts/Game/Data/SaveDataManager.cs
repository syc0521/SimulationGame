using System.Collections.Generic;
using System.IO;
using Game.Core;
using Game.Data.Event;

namespace Game.Data
{
    public class SaveDataManager : ManagerBase, ISaveDataManager
    {
        private PlayerData _playerData;
        private const string Path = "D://1.txt";

        public override void OnStart()
        {
            LoadData();
        }

        public void SaveData()
        {
            var bytes = MessagePack.MessagePackSerializer.Serialize(_playerData);
            File.WriteAllBytes(Path, bytes);
        }

        private void LoadData()
        {
            try
            {
                var bytes = File.ReadAllBytes(Path);
                _playerData = MessagePack.MessagePackSerializer.Deserialize<PlayerData>(bytes);
            }
            catch (FileNotFoundException e)
            {
                _playerData = new()
                {
                    buildings = new(),
                    tasks = new()
                };
                EventCenter.DispatchEvent(new InitializeSaveDataEvent());
            }
            finally
            {
                EventCenter.DispatchEvent(new LoadDataEvent());
                SaveData();
            }
        }

        public Dictionary<uint, BuildingData> GetBuildings()
        {
            return _playerData.buildings ?? (_playerData.buildings = new());
        }
        
        public void GetPlayerTasks(ref Dictionary<int, PlayerTaskData> taskData)
        {
            taskData = _playerData.tasks;
        }
    }
}