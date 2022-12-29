﻿using System.Collections.Generic;
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
            var bytes = File.ReadAllBytes(Path);
            if (bytes.Length == 0)
            {
                return;
            }
            _playerData = MessagePack.MessagePackSerializer.Deserialize<PlayerData>(bytes);
        }

        public Dictionary<uint, BuildingData> GetBuildings()
        {
            return _playerData.buildings ?? (_playerData.buildings = new());
        }
    }
}