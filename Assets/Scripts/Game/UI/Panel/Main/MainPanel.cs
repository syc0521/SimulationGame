﻿using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Currency;
using Game.Data.Event.Task;
using Game.GamePlaySystem.Currency;
using Game.UI.Component;
using Game.UI.Panel.Bag;
using Game.UI.Panel.Building;
using Game.UI.UISystem;

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

        public override void OnCreated()
        {
            nodes.build_btn.onClick.AddListener(OpenBuildPanel);
            nodes.bag_btn.onClick.AddListener(OpenBagPanel);
            nodes.destroy_btn.onClick.AddListener(DestroyHandler);
        }

        public override void OnShown()
        {
            base.OnShown();
            EventCenter.AddListener<DataChangedEvent>(RefreshUI);
            EventCenter.AddListener<RefreshUITaskEvent>(RefreshTask);
            EventCenter.AddListener<BuildUIEvent>(ShowConfirmUI);
            EventCenter.AddListener<UpdateCurrencyEvent>(RefreshCurrency);
            RefreshCurrency(default);
        }

        public override void OnDestroyed()
        {
            nodes.build_btn.onClick.RemoveListener(OpenBuildPanel);
            nodes.bag_btn.onClick.RemoveListener(OpenBagPanel);
            nodes.destroy_btn.onClick.RemoveListener(DestroyHandler);

            EventCenter.RemoveListener<DataChangedEvent>(RefreshUI);
            EventCenter.RemoveListener<RefreshUITaskEvent>(RefreshTask);
            EventCenter.RemoveListener<BuildUIEvent>(ShowConfirmUI);
            EventCenter.RemoveListener<UpdateCurrencyEvent>(RefreshCurrency);
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

        private void RefreshUI(DataChangedEvent evt)
        {
            RefreshData(evt.gameData);
        }

        private void RefreshData(GameData data)
        {
            nodes.people_txt.text = data.people.ToString();
        }

        private void RefreshCurrency(UpdateCurrencyEvent evt)
        {
            var coin = CurrencyManager.Instance.GetCurrency(CurrencyType.Coin);
            nodes.money_txt.text = coin.ToString();
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

    }
}