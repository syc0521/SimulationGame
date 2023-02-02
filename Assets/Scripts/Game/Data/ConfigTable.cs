using System;
using System.Collections.Generic;
using Game.Core;
using Game.Data.ScriptableObject;
using Game.Data.TableData;
using UnityEngine;

namespace Game.Data
{
    public class ConfigTable : Singleton<ConfigTable>
    {
        [SerializeField] private Transform uiRoot;
        [SerializeField] private BuildingCollection buildings;
        [SerializeField] private Reward reward;
        [SerializeField] private Task task;
        [SerializeField] private Building building;
        [SerializeField] private UIPanelTable panelTable;

        public Transform GetUIRoot(int layerType) => uiRoot.GetChild(layerType);

        public GameObject GetBuilding(int id) => buildings.buildings[id];

        public List<TaskData> GetTasks() => task.dataList;

        public TaskData GetTask(int id) => task.dataList.Find(item => item.Taskid == id);

        public TableData.RewardData GetRewardData(int id) => reward.dataList.Find(item => item.Rewardid == id);

        public List<TableData.BuildingData> GetAllBuildingData() => building.dataList;
        
        public TableData.BuildingData GetBuildingData(int id) => building.dataList[id];

        public UIPanelTableData GetUIPanelData(string panelName) => panelTable.dataList.Find(item => item.Name == panelName);

    }
}