using System;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.Data.Event.Task;
using Game.GamePlaySystem.Task;
using Game.UI.Component;
using Game.UI.Panel.Task;
using Game.UI.UISystem;
using TMPro;
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

        public override void OnCreated()
        {
            EventCenter.AddListener<DataChangedEvent>(RefreshUI);
            EventCenter.AddListener<RefreshUITaskEvent>(RefreshTask);
        }

        public override void OnDestroyed()
        {
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
            nodes.text.text = evt.gameData.people.ToString();
        }

        
    }
}