﻿using System.Collections.Generic;
using System.IO;
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
                _playerData = new()
                {
                    buildings = new(),
                    tasks = new(),
                    backpack = new(),
                    currency = new(){ {0,0},{1,0},{2,0} },
                };
                EventCenter.DispatchEvent(new InitializeSaveDataEvent());
            }
            finally
            {
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

        public void ResetSaveData()
        {
            _playerData = new()
            {
                buildings = new(),
                tasks = new(),
                backpack = new(),
                currency = new(){ {0,0},{1,0},{2,0} },
            };
            EventCenter.DispatchEvent(new InitializeSaveDataEvent());
            SaveData();
        }
    }
}