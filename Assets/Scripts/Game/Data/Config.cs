using System.Collections.Generic;
using Game.Core;
using Game.Data.ScriptableObject;
using Game.Data.TableData;
using UnityEngine;

namespace Game.Data
{
    public class Config : Singleton<Config>
    {
        [SerializeField] private PanelDef panelDef;
        [SerializeField] private Transform uiRoot;
        [SerializeField] private BuildingCollection buildings;
        [SerializeField] private Reward reward;
        [SerializeField] private Task task;

        public GameObject GetPanel(PanelEnum type) => panelDef.panels[type];

        public Transform GetUIRoot() => uiRoot;

        public List<GameObject> GetBuildings() => buildings.buildings;

        public List<TableData.TaskData> GetTasks() => task.dataList;

        public TableData.RewardData GetRewardData(int id) => reward.dataList.Find(item => item.Rewardid == id);

    }
}