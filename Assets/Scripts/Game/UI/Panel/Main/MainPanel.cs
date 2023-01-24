using System;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.Data.Event.Task;
using Game.GamePlaySystem;
using Game.GamePlaySystem.Task;
using Game.UI.Component;
using Game.UI.Panel.Building;
using Game.UI.Panel.Task;
using Game.UI.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            nodes.destroy_btn.onClick.AddListener(DestroyHandler);

            EventCenter.AddListener<DataChangedEvent>(RefreshUI);
            EventCenter.AddListener<RefreshUITaskEvent>(RefreshTask);
        }

        public override void OnDestroyed()
        {
            nodes.build_btn.onClick.AddListener(OpenBuildPanel);
            
            EventCenter.RemoveListener<DataChangedEvent>(RefreshUI);
            EventCenter.RemoveListener<RefreshUITaskEvent>(RefreshTask);
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

        public void RefreshUI(DataChangedEvent evt)
        {
            nodes.people_txt.text = evt.gameData.people.ToString();
            nodes.money_txt.text = evt.gameData.money.ToString();
        }

        private void OpenBuildPanel()
        {
            UIManager.Instance.OpenPanel<BuildingPanel>();
        }
        
        private void DestroyHandler()
        {
            UIManager.Instance.OpenPanel<DestroyBuildingPanel>();
            transform.gameObject.SetActive(false);
        }

    }
}