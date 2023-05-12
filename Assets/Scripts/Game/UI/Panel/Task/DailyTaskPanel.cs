using System.Collections.Generic;
using Game.Audio;
using Game.Core;
using Game.Data;
using Game.GamePlaySystem.Task;
using Game.UI.Component;
using Game.UI.UISystem;
using Game.UI.ViewData;

namespace Game.UI.Panel.Task
{
    public class DailyTaskListData : ListData
    {
        public int taskId;
        public string name, desc;
        public TaskState state;
    }
    
    public class DailyTaskPanel : UIPanel
    {
        public DailyTaskPanel_Nodes nodes;
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.back_btn.onClick.AddListener(Close);
        }

        public override void OnShown()
        {
            base.OnShown();
            PlayAnimation();
            GetDailyTask();
        }

        public override void OnDestroyed()
        {
            nodes.back_btn.onClick.RemoveListener(Close);
            base.OnDestroyed();
        }

        private void GetDailyTask()
        {
            var dailyTask = TaskSystem.Instance.GetDailyTaskData();
            foreach (var task in dailyTask)
            {
                nodes.task_list.AddItem(new DailyTaskListData
                {
                    name = task.Value.name,
                    desc = task.Value.desc,
                    taskId = task.Key,
                    state = TaskManager.Instance.GetTaskState(task.Key)
                });
            }
        }

        private void Close()
        {
            Managers.Get<IAudioManager>().PlaySFX(SFXType.Button2);
            CloseSelf();
        }
    }
}