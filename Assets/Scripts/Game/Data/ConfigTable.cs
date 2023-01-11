using System.Collections.Generic;
using Game.Core;
using Game.Data.ScriptableObject;
using Game.Data.TableData;
using UnityEngine;

namespace Game.Data
{
    public class ConfigTable : Singleton<ConfigTable>
    {
        [SerializeField] private PanelDef panelDef;
        [SerializeField] private Transform uiRoot;
        [SerializeField] private BuildingCollection buildings;
        [SerializeField] private Reward reward;
        [SerializeField] private Task task;
        [SerializeField] private Building building;

        public GameObject GetPanel(PanelEnum type) => panelDef.panels[type];

        public Transform GetUIRoot() => uiRoot;

        public GameObject GetBuilding(int id) => buildings.buildings[id];

        public List<TableData.TaskData> GetTasks() => task.dataList;

        public TableData.RewardData GetRewardData(int id) => reward.dataList.Find(item => item.Rewardid == id);

        public List<TableData.BuildingData> GetBuildingData() => building.dataList;

    }
}