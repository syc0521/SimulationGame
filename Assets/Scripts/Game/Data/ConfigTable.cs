using System;
using System.Collections.Generic;
using Game.Core;
using Game.Data.FeatureOpen;
using Game.Data.ScriptableObject;
using Game.Data.TableData;
using UnityEngine;

namespace Game.Data
{
    public class ConfigTable : Singleton<ConfigTable>
    {
        [SerializeField] private Transform uiRoot;
        [SerializeField] private BuildingCollection buildings;
        [SerializeField] private RewardGroup reward;
        [SerializeField] private Task task;
        [SerializeField] private Building building;
        [SerializeField] private UIPanelTable panelTable;
        [SerializeField] private GestureConfig gestureConfig;
        [SerializeField] private BuildConfig buildConfig;
        [SerializeField] private BagItem bagItem;
        [SerializeField] private Currency currency;
        [SerializeField] private BuildingProduce buildingProduce;
        [SerializeField] private HouseConsume houseConsume;
        [SerializeField] private TableData.FeatureOpen featureOpen;
        [SerializeField] private LoadingStep loadingStep;

        public Transform GetUIRoot(int layerType) => uiRoot.GetChild(layerType);

        public GameObject GetBuilding(int id) => buildings.buildings[id];

        public List<TaskData> GetTasks() => task.dataList;

        public TaskData GetTask(int id) => task.dataList.Find(item => item.Taskid == id);

        public RewardGroupData GetRewardGroupData(int id) => reward.dataList.Find(item => item.Rewardid == id);

        public List<TableData.BuildingData> GetAllBuildingData() => building.dataList;
        
        public TableData.BuildingData GetBuildingData(int id) => building.dataList[id];

        public UIPanelTableData GetUIPanelData(string panelName) => panelTable.dataList.Find(item => item.Name == panelName);

        public GestureConfig GetGestureConfig() => gestureConfig;

        public BuildConfig GetBuildConfig() => buildConfig;

        public BagItemData GetBagItemData(int id) => bagItem.dataList.Find(item => item.ID == id);

        public CurrencyData GetCurrencyData(int id) => currency.dataList.Find(item => item.ID == id);

        public BuildingProduceData GetBuildingProduceData(int id) => buildingProduce.dataList[id];

        public HouseConsumeData GetHouseConsumeData(int id, int level) =>
            houseConsume.dataList.Find(item => item.Buildingid == id && item.Level == level);

        public FeatureOpenData GetFeatureOpenData(FeatureType type) => featureOpen.dataList.Find(item => item.Featureid == (int)type);

        public LoadingStepData GetLoadingStepData(int stepID) => loadingStep.dataList.Find(item => item.Stepid == stepID);
    }
}