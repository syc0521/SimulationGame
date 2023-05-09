using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Event.Common;
using Game.Data.Event.Task;
using Game.Data.TableData;
using Game.GamePlaySystem.Task;
using Game.UI.ViewData;

namespace Game.UI.UISystem
{
    public class TaskSystem : UISystemBase<TaskSystem>
    {
        private Dictionary<int, TaskViewData> _taskData = new();
        private Dictionary<int, DailyTaskViewData> _dailyTaskData = new();

        public override void OnAwake()
        {
            base.OnAwake();
            EventCenter.AddListener<RefreshTaskEvent>(RefreshTask);
            EventCenter.AddListener<LoadSceneFinishedEvent>(InitDailyTaskData);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<RefreshTaskEvent>(RefreshTask);
            EventCenter.RemoveListener<LoadSceneFinishedEvent>(InitDailyTaskData);
            _taskData.Clear();
            _taskData = null;
            base.OnDestroyed();
        }

        private void RefreshTask(RefreshTaskEvent evt)
        {
            _taskData.Clear();
            var playerTask = evt.playerTask;
            foreach (var task in playerTask)
            {
                var taskTableData = ConfigTable.Instance.GetTask(task.Key);
                _taskData[task.Key] = new TaskViewData
                {
                    name = taskTableData.Name,
                    reward = taskTableData.Reward == -1 ? null : 
                        GetRewardData(ConfigTable.Instance.GetRewardGroupData(taskTableData.Reward)),
                    content = taskTableData.Content,
                    state = task.Value.state,
                    targetID = taskTableData.Targetid,
                    targetNum = taskTableData.Targetnum,
                    currentNum = task.Value.currentNum,
                    type = (TaskType)taskTableData.Tasktype,
                };
            }
            
            EventCenter.DispatchEvent(new RefreshUITaskEvent());
        }

        public Dictionary<int, TaskViewData> GetAllTaskData() => _taskData;

        public TaskViewData GetTaskData(int taskID) => _taskData.ContainsKey(taskID) ? _taskData[taskID] : null;

        private List<RewardData> GetRewardData(RewardGroupData rewardGroup)
        {
            List<RewardData> rewardData = new(0);
            rewardData.AddRange(rewardGroup.Rewardtype.Select((t, i) => new RewardData
            {
                type = (RewardType)t, itemID = rewardGroup.Itemid[i], amount = rewardGroup.Count[i],
            }));
            return rewardData;
        }

        private void InitDailyTaskData(LoadSceneFinishedEvent evt)
        {
            var taskIds = TaskManager.Instance.GetDailyTask();
            foreach (var taskId in taskIds)
            {
                var taskData = ConfigTable.Instance.GetTask(taskId);
                _dailyTaskData[taskId] = new DailyTaskViewData
                {
                    name = taskData.Name,
                    desc = taskData.Content,
                };
            }
        }

        public Dictionary<int, DailyTaskViewData> GetDailyTaskData() => _dailyTaskData;
    }
}