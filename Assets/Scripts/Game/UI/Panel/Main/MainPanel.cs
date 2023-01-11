using System;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem.Task;
using Game.UI.Component;
using Game.UI.Panel.Task;
using TMPro;

namespace Game.UI.Panel
{
    public class TaskListData : ListData
    {
        public int id;
        public TaskState state;
        public int currentNum;
    }
    public class MainPanel : UIPanel
    {
        public MainPanel_Nodes nodes;

        public override void OnCreated()
        {
            EventCenter.AddListener<DataChangedEvent>(RefreshUI);
            InitTask();
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<DataChangedEvent>(RefreshUI);
        }

        private void InitTask()
        {
            var tasks = TaskManager.Instance.GetPlayerTask();
            foreach (var task in tasks)
            {
                nodes.task_list.AddItem(new TaskListData
                {
                    id = task.Key,
                    state = task.Value.state,
                    currentNum = task.Value.currentNum
                });
            }
        }

        public void RefreshUI(DataChangedEvent evt)
        {
            nodes.text.text = evt.gameData.people.ToString();
        }

        
    }
}