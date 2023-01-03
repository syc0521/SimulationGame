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
                InitializeTask();
            }
        }
        
        private void InitializeTask()
        {
            var beginnerTask = Config.Instance.GetTasks().FindAll(item => item.Previousid == -1);
            foreach (var task in beginnerTask)
            {
                ActivateTask(task.Taskid);
            }
        }

        public Dictionary<uint, BuildingData> GetBuildings()
        {
            return _playerData.buildings ?? (_playerData.buildings = new());
        }

        public TaskState GetTaskState(int id)
        {
            return _playerData.tasks.ContainsKey(id) ? _playerData.tasks[id].state : TaskState.Error;
        }

        public void ActivateTask(int id)
        {
            _playerData.tasks[id] = new PlayerTaskData
            {
                state = TaskState.Accepted
            };
            Debug.LogError($"已开启id为{id}的任务");
            SaveData();
        }

        public bool ChangeTaskState(int id, TaskState state)
        {
            if (_playerData.tasks.ContainsKey(id))
            {
                _playerData.tasks[id].state = state;
                SaveData();
                return true;
            }

            return false;
        }

        public bool ChangeTaskNum(int id, int num)
        {
            if (_playerData.tasks.ContainsKey(id))
            {
                _playerData.tasks[id].currentNum += num;
                SaveData();
                return true;
            }
            return false;
        }

        public int GetTaskNum(int id)
        {
            if (_playerData.tasks.ContainsKey(id))
            {
                return _playerData.tasks[id].currentNum;
            }
            return -1;
        }


    }
}