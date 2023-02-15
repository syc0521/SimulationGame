﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game.Core;
using Game.Data.Event;
using UnityEngine;

namespace Game.Data
{
    public class SaveDataManager : ManagerBase, ISaveDataManager
    {
        private PlayerData _playerData;

        public override void OnStart()
        {
            Debug.Log(GetPath());
            LoadData();
        }

        private static string GetPath()
        {
            return $"{Application.persistentDataPath}/save.data";
        }

        public void SaveData()
        {
            var bytes = MessagePack.MessagePackSerializer.Serialize(_playerData);
            File.WriteAllBytes(GetPath(), bytes);
        }

        private void LoadData()
        {
            try
            {
                var bytes = File.ReadAllBytes(GetPath());
                _playerData = MessagePack.MessagePackSerializer.Deserialize<PlayerData>(bytes);
            }
            catch (FileNotFoundException)
            {
                InitializeSaveData();
            }
            finally
            {
                ProcessData();
                EventCenter.DispatchEvent(new LoadDataEvent());
                SaveData();
            }
        }

        public void GetBuildings(ref Dictionary<uint, BuildingData> buildings)
        {
            buildings = _playerData.buildings;
        }
        
        public void GetPlayerTasks(ref Dictionary<int, PlayerTaskData> taskData)
        {
            taskData = _playerData.tasks;
        }

        public void GetBackpack(ref Dictionary<int, int> backpack)
        {
            backpack = _playerData.backpack;
        }

        public void GetCurrency(ref Dictionary<int, int> currency)
        {
            currency = _playerData.currency;
        }

        public void GetUnlockedBuildings(ref HashSet<int> unlockedBuildings)
        {
            unlockedBuildings = _playerData.unlockedBuildings;
        }

        public void ResetSaveData()
        {
            InitializeSaveData();
            SaveData();
        }

        private void InitializeSaveData()
        {
            var unlockList = ConfigTable.Instance.GetAllBuildingData().FindAll(item => item.Unlock);
            _playerData = new()
            {
                buildings = new(),
                tasks = new(),
                backpack = new(){ {0,5},{10,5} },
                currency = new(){ {0,0},{1,0},{2,0} },
                unlockedBuildings = unlockList.Select(item => item.Buildingid).ToHashSet()
            };
            EventCenter.DispatchEvent(new InitializeSaveDataEvent());
        }

        private void ProcessData() // 主要处理不同游戏版本间存档不互通导致循环报错的问题
        {
            if (_playerData.backpack == null || _playerData.backpack.Count == 0)
            {
                _playerData.backpack = new(){ {0,5}, {10,5} };
            }

            if (_playerData.currency == null || _playerData.currency.Count == 0)
            {
                _playerData.currency = new(){ {0,0},{1,0},{2,0} };
            }

            if (_playerData.unlockedBuildings == null || _playerData.unlockedBuildings.Count == 0)
            {
                var unlockList = ConfigTable.Instance.GetAllBuildingData().FindAll(item => item.Unlock);
                _playerData.unlockedBuildings = unlockList.Select(item => item.Buildingid).ToHashSet();
            }
        }
    }
}