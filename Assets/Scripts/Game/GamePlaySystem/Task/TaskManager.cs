using System.Collections.Generic;
using Game.Core;
using Game.Data;
using UnityEngine;
using TaskData = Game.Data.TableData.TaskData;

namespace Game.GamePlaySystem.Task
{
    public static class TaskDecorator
    {
        public static TaskState GetTaskState(this TaskData taskData)
        {
            return Managers.Get<ISaveDataManager>().GetTaskState(taskData.Taskid);
        }
    }
    
    public class TaskManager : GamePlaySystemBase<TaskManager>
    {
        public override void OnStart()
        {
            
        }

        private void ActivateTask(int id)
        {
            Managers.Get<ISaveDataManager>().ActivateTask(id);
        }

        public void TriggerTask(TaskType taskType, int targetID, int targetNum)
        {
            var runningTasks = ConfigTable.Instance.GetTasks().FindAll(item =>
                item.GetTaskState() == TaskState.Accepted && item.Tasktype == (int)taskType && item.Targetid == targetID);
            foreach (var task in runningTasks)
            {
                Managers.Get<ISaveDataManager>().ChangeTaskNum(task.Taskid, targetNum);
                if (Managers.Get<ISaveDataManager>().GetTaskNum(task.Taskid) >= task.Targetnum)
                {
                    Managers.Get<ISaveDataManager>().ChangeTaskState(task.Taskid, TaskState.Finished);
                    GetReward(targetID);
                }
            }
        }

        public void GetReward(int targetID)
        {
            Managers.Get<ISaveDataManager>().ChangeTaskState(targetID, TaskState.Rewarded);
            Debug.LogError($"已领取ID为{targetID}的奖励");
            
            var nextTasks = ConfigTable.Instance.GetTasks().FindAll(item => item.Previousid == targetID);
            foreach (var task in nextTasks)
            {
                ActivateTask(task.Taskid);
            }
        }

        // 如果任务变动用事件，第一次进游戏获取任务用这个
        public Dictionary<int, PlayerTaskData> GetPlayerTask()
        {
            return Managers.Get<ISaveDataManager>().GetCurrentTasks();
        }
    }
}