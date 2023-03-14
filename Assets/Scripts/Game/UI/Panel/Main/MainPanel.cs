﻿using System;
using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Common;
using Game.Data.Event.Currency;
using Game.Data.Event.FeatureOpen;
using Game.Data.Event.Task;
using Game.Data.FeatureOpen;
using Game.GamePlaySystem;
using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Currency;
using Game.GamePlaySystem.FeatureOpen;
using Game.LevelAndEntity.System;
using Game.UI.Component;
using Game.UI.Decorator;
using Game.UI.Panel.Bag;
using Game.UI.Panel.Building;
using Game.UI.Panel.Pause;
using Game.UI.UISystem;
using Game.UI.Widget;
using Unity.Entities;
using UnityEngine;

namespace Game.UI.Panel
{
    public class TaskListData : ListData
    {
        public int id;
        public TaskState state;
    }
    public class MainPanel : UIPanel
    {
        public MainPanel_Nodes nodes;
        private const float Interval = 0.5f;
        private float _time = 0f;

        public override void OnCreated()
        {
            nodes.build_btn.onClick.AddListener(OpenBuildPanel);
            nodes.bag_btn.onClick.AddListener(OpenBagPanel);
            nodes.destroy_btn.onClick.AddListener(DestroyHandler);
            nodes.pause_btn.onClick.AddListener(ShowPausePanel);
            nodes.closeTip_btn.onClick.AddListener(CloseTip);
            
            nodes.happiness_widget.SetClickHandler(ShowTip, StatusType.Happiness);
            nodes.money_widget.SetClickHandler(ShowTip, StatusType.Coin);
            nodes.people_widget.SetClickHandler(ShowTip, StatusType.People);
        }

        public override void OnShown()
        {
            base.OnShown();
            EventCenter.AddListener<RefreshUITaskEvent>(RefreshTask);
            EventCenter.AddListener<BuildUIEvent>(ShowConfirmUI);
            EventCenter.AddListener<UpdateCurrencyEvent>(RefreshCurrency);
            EventCenter.AddListener<UnlockFeatureEvent>(RefreshFeatureButtons);
            EventCenter.AddListener<OpenBuildingInfoEvent>(OpenBuildingInfo);
            EventCenter.AddListener<FPSEvent>(ShowFPS);

            RefreshFeatureButtons(default);
            RefreshCurrency(default);
            RefreshTask(default);
        }

        public override void OnDestroyed()
        {
            nodes.build_btn.onClick.RemoveListener(OpenBuildPanel);
            nodes.bag_btn.onClick.RemoveListener(OpenBagPanel);
            nodes.destroy_btn.onClick.RemoveListener(DestroyHandler);
            nodes.pause_btn.onClick.RemoveListener(ShowPausePanel);
            nodes.closeTip_btn.onClick.RemoveListener(CloseTip);

            EventCenter.RemoveListener<RefreshUITaskEvent>(RefreshTask);
            EventCenter.RemoveListener<BuildUIEvent>(ShowConfirmUI);
            EventCenter.RemoveListener<UpdateCurrencyEvent>(RefreshCurrency);
            EventCenter.RemoveListener<UnlockFeatureEvent>(RefreshFeatureButtons);
            EventCenter.RemoveListener<OpenBuildingInfoEvent>(OpenBuildingInfo);
            EventCenter.RemoveListener<FPSEvent>(ShowFPS);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _time += Time.deltaTime;
            if (_time >= Interval)
            {
                _time = 0f;
                RefreshUI();
            }
        }

        private void RefreshFeatureButtons(UnlockFeatureEvent evt)
        {
            nodes.bag_btn.gameObject.SetActive(FeatureOpenManager.Instance.HasFeature(FeatureType.Backpack));
            nodes.destroy_btn.gameObject.SetActive(FeatureOpenManager.Instance.HasFeature(FeatureType.Destroy));
            nodes.operate_widget.RefreshButtons();
        }

        private void RefreshTask(RefreshUITaskEvent evt)
        {
            nodes.task_list.Clear();
            var tasks = TaskSystem.Instance.GetAllTaskData();
            foreach (var task in tasks)
            {
                nodes.task_list.AddItem(new TaskListData
                {
                    id = task.Key,
                    state = task.Value.state,
                });
            }
        }

        private void RefreshUI()
        {
            var dataSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DataSystem>();
            if (dataSystem == null)
            {
                return;
            }
            var data = dataSystem.GetGameData();
            if (data.Equals(default))
            {
                return;
            }
            
            nodes.people_widget.SetText(data.people.ToString());
            nodes.happiness_widget.SetText(string.Format($"{(int)(data.happiness * 100)}%"));
        }

        private void RefreshCurrency(UpdateCurrencyEvent evt)
        {
            var coin = CurrencyManager.Instance.GetCurrency(CurrencyType.Coin);
            nodes.money_widget.SetText(coin.ToString());
        }

        private void OpenBuildPanel()
        {
            UIManager.Instance.OpenPanel<BuildingPanel>();
        }

        private void OpenBagPanel()
        {
            UIManager.Instance.OpenPanel<BagPanel>();
        }
        
        private void DestroyHandler()
        {
            UIManager.Instance.OpenPanel<DestroyBuildingPanel>();
        }
        
        private void ShowConfirmUI(BuildUIEvent obj)
        {
            if (!nodes.operate_widget.gameObject.activeInHierarchy)
            {
                nodes.operate_widget.gameObject.SetActive(true);
            }
            
            nodes.operate_widget.ShowConfirmButton(obj.canConstruct);
        }

        private void ShowPausePanel()
        {
            UIManager.Instance.OpenPanel<PausePanel>();
        }

        private void ShowTip(StatusWidget widget)
        {
            switch (widget.Type)
            {
                case StatusType.Happiness:
                    nodes.tip_w.SetTitle("满意度");
                    nodes.tip_w.SetDescription("建造建筑，提高供给,可提高满意度");
                    break;
                case StatusType.People:
                    nodes.tip_w.SetTitle("人口");
                    nodes.tip_w.SetDescription("临安城目前居住的人口，受满意度控制");
                    break;
                case StatusType.Coin:
                    nodes.tip_w.SetTitle("通宝");
                    nodes.tip_w.SetDescription("宋代的普通货币");
                    break;
            }
            nodes.tip_w.gameObject.SetActive(true);
            nodes.tip_w.transform.position = widget.transform.position - new Vector3(25, 100);
            nodes.closeTip_btn.gameObject.SetActive(true);
        }

        private void CloseTip()
        {
            nodes.tip_w.gameObject.SetActive(false);
            nodes.closeTip_btn.gameObject.SetActive(false);
            nodes.buildingDetail_w.gameObject.SetActive(false);
        }
        
        private void OpenBuildingInfo(OpenBuildingInfoEvent evt)
        {
            if (evt.isStatic)
            {
                if (evt.id == 1) // todo 打开官府面板
                {
                    
                }
            }
            else
            {
                var data = BuildingManager.Instance.GetBuildingData((uint)evt.id);
                var buildingData = ConfigTable.Instance.GetBuildingData(data.type);
                if (buildingData == null || buildingData.Buildingid == 4)
                {
                    return;
                }
            
                nodes.buildingDetail_w.SetDefault();
                nodes.buildingDetail_w.gameObject.SetActive(true);
                nodes.closeTip_btn.gameObject.SetActive(true);
            
                nodes.buildingDetail_w.SetTitle(buildingData.Name);
                nodes.buildingDetail_w.SetDescription(buildingData.Description);
                if (buildingData.Level > 1)
                {
                    nodes.buildingDetail_w.SetLevel(data.level);
                    if (FeatureOpenManager.Instance.HasFeature(FeatureType.Upgrade))
                    {
                        nodes.buildingDetail_w.SetUpgradeState(data.level < buildingData.Level);
                    }
                    nodes.buildingDetail_w.SetUpgradeHandler(UpgradeHandler);
                    
                    void UpgradeHandler()
                    {
                        if (data.level == buildingData.Level) return;

                        var upgradeData = ConfigTable.Instance.GetBuildingUpgradeData(data.type, data.level + 1);
                        bool canUpgrade = true;
                        for (var i = 0; i < upgradeData.Currencyid.Length; i++)
                        {
                            var count = CurrencyManager.Instance.GetCurrency((CurrencyType)upgradeData.Currencyid[i]);
                            if (count < upgradeData.Currencycount[i])
                            {
                                canUpgrade = false;
                            }
                        }

                        for (var i = 0; i < upgradeData.Itemid.Length; i++)
                        {
                            var count = BackpackManager.Instance.GetBackpackCount(upgradeData.Itemid[i]);
                            if (count < upgradeData.Itemcount[i])
                            {
                                canUpgrade = false;
                            }
                        }

                        if (canUpgrade)
                        {
                            BuildingManager.Instance.UpgradeBuilding((uint)evt.id, data.level + 1, evt.isStatic);
                        }
                        else
                        {
                            AlertDecorator.OpenAlertPanel("货币或材料不足！", false);
                        }
                    }
                }

                if (buildingData.Cd > 0)
                {
                    var produceData = ConfigTable.Instance.GetBuildingProduceData(data.type);
                    var itemPerMin = produceData.Produceamount[data.level - 1] / buildingData.Cd * 60.0f;
                    nodes.buildingDetail_w.SetProduceAmount((int)itemPerMin);
                }
            }
        }

        private void ShowFPS(FPSEvent evt)
        {
            nodes.fps_txt.text = $"FPS: {evt.fps}";
        }

    }
}