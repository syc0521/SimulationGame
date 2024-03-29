﻿using System;
using System.Collections.Generic;
using Game.Core;
using Game.Data.FeatureOpen;

namespace Game.Data
{
    public interface ISaveDataManager : IManager
    {
        void LoadData();
        void SaveData();
        void ResetSaveData();
        void GetBuildings(ref Dictionary<uint, BuildingData> buildings);
        Dictionary<uint, BuildingData> GetBuildings();
        void GetPlayerTasks(ref Dictionary<int, PlayerTaskData> taskData);
        void GetBackpack(ref Dictionary<int, int> backpack);
        void GetCurrency(ref Dictionary<int, int> currency);
        void GetUnlockedBuildings(ref HashSet<int> unlockedBuildings);
        void GetSettingData(ref SettingData data);
        void GetUnlockedFeatures(ref HashSet<FeatureType> unlockedFeatures);
        void GetPlayerAchievement(ref Dictionary<int, PlayerAchievementData> playerAchievement);
        DateTime GetLastLoginTime();
    }
}