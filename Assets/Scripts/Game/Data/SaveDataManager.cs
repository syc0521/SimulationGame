using System;
using System.Collections.Generic;
using System.IO;
using Game.Core;
using UnityEngine;

namespace Game.Data
{
    public class SaveDataManager : ManagerBase, ISaveDataManager
    {
        private PlayerData _playerData;
        private const string Path = "D://1.txt";

        public override void OnAwake()
        {
            LoadData();
        }

        public void SaveData()
        {
            var bytes = MessagePack.MessagePackSerializer.Serialize(_playerData);
            File.WriteAllBytes(Path, bytes);
        }

        public void LoadData()
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
                
            }
            
        }

        public Dictionary<uint, BuildingData> GetBuildings()
        {
            return _playerData.buildings ?? (_playerData.buildings = new());
        }

        public TaskState GetTaskState(uint id)
        {
            return _playerData.tasks.ContainsKey(id) ? _playerData.tasks[id].state : TaskState.Error;
        }

        public void ActivateTask(uint id)
        {
            _playerData.tasks[id].SetState(TaskState.Accepted);
        }

        public bool ChangeTaskState(uint id, TaskState state)
        {
            if (_playerData.tasks.ContainsKey(id))
            {
                _playerData.tasks[id].SetState(state);
                return true;
            }

            return false;
        }

        public bool ChangeTaskNum(uint id, int num)
        {
            if (_playerData.tasks.ContainsKey(id))
            {
                _playerData.tasks[id].SetNum(num);
                return true;
            }
            return false;
        }

        public int GetTaskNum(uint id)
        {
            if (_playerData.tasks.ContainsKey(id))
            {
                return _playerData.tasks[id].currentNum;
            }
            return -1;
        }


    }
}