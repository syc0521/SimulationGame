using System.Collections.Generic;
using Game.Core;
using Game.Data;

namespace Game.GamePlaySystem.Task
{
    public class TaskManager : GamePlaySystemBase<TaskManager>
    {
        private List<TaskData> tasks;
        public override void OnStart()
        {
            tasks = new();
            tasks.Add(new TaskData
            {
                taskID = 0,
                name = "新手引导1",
                previousID = -1,
                taskType = TaskType.AddBuilding,
                targetID = 0,
                targetNum = 2,
            });
            tasks.Add(new TaskData
            {
                taskID = 1,
                name = "新手引导2",
                previousID = 0,
                taskType = TaskType.AddBuilding,
                targetID = 1,
                targetNum = 2,
            });
        }

        public void InitializeTask()
        {
            var beginnerTask = tasks.FindAll(item => item.previousID == -1);
            foreach (var task in beginnerTask)
            {
                ActivateTask(task.taskID);
            }
        }

        public void ActivateTask(uint id)
        {
            Managers.Get<ISaveDataManager>().ActivateTask(id);
        }

        public void TriggerTask(TaskType taskType, uint targetID, int targetNum)
        {
            var runningTasks = tasks.FindAll(item =>
                item.TaskState == TaskState.Accepted && item.taskType == taskType && item.targetID == targetID);
            foreach (var task in runningTasks)
            {
                Managers.Get<ISaveDataManager>().ChangeTaskNum(targetID, targetNum);
                if (Managers.Get<ISaveDataManager>().GetTaskNum(targetID) >= task.targetNum)
                {
                    Managers.Get<ISaveDataManager>().ChangeTaskState(targetID, TaskState.Finished);
                }
            }
        }

        public void GetReward(uint targetID)
        {
            Managers.Get<ISaveDataManager>().ChangeTaskState(targetID, TaskState.Rewarded);
            var nextTasks = tasks.FindAll(item => item.previousID == targetID);
            foreach (var task in nextTasks)
            {
                ActivateTask(task.taskID);
            }
        }
    }
}