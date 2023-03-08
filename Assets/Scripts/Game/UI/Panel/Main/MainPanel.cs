using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Currency;
using Game.Data.Event.FeatureOpen;
using Game.Data.Event.Task;
using Game.Data.FeatureOpen;
using Game.GamePlaySystem.Currency;
using Game.GamePlaySystem.FeatureOpen;
using Game.LevelAndEntity.System;
using Game.UI.Component;
using Game.UI.Panel.Bag;
using Game.UI.Panel.Building;
using Game.UI.Panel.Pause;
using Game.UI.UISystem;
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
            
            nodes.environment_widget.SetClickHandler(ShowTip);
            nodes.happiness_widget.SetClickHandler(ShowTip);
            nodes.money_widget.SetClickHandler(ShowTip);
            nodes.people_widget.SetClickHandler(ShowTip);
        }

        public override void OnShown()
        {
            base.OnShown();
            EventCenter.AddListener<RefreshUITaskEvent>(RefreshTask);
            EventCenter.AddListener<BuildUIEvent>(ShowConfirmUI);
            EventCenter.AddListener<UpdateCurrencyEvent>(RefreshCurrency);
            EventCenter.AddListener<UnlockFeatureEvent>(RefreshFeatureButtons);
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
            nodes.environment_widget.SetText(string.Format($"{(int)(data.environment * 100)}%"));
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

        private void ShowTip(Transform widgetTransform)
        {
            nodes.tip_w.gameObject.SetActive(true);
            nodes.tip_w.transform.position = widgetTransform.position - new Vector3(0, -50);
            nodes.closeTip_btn.gameObject.SetActive(true);
        }

        private void CloseTip()
        {
            nodes.tip_w.gameObject.SetActive(false);
        }

    }
}