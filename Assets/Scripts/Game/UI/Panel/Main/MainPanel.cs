using System;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.Task;
using Game.UI.Component;
using Game.UI.Panel.Task;
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
        }

        public override void OnShown()
        {
            base.OnShown();
            InitTask();
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<DataChangedEvent>(RefreshUI);
        }

        private void InitTask()
        {
            var tasks = TaskManager.Instance.GetPlayerTask();
            foreach (var task in 
                     tasks.Where(item => item.Value.state is TaskState.Accepted))
            {
                Debug.Log(task.Value.state);
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