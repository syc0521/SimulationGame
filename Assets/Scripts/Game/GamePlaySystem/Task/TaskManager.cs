using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.Data.Event.Task;
using UnityEngine;
using TaskData = Game.Data.TableData.TaskData;

namespace Game.GamePlaySystem.Task
{
    public static class TaskDecorator
    {
        public static TaskState GetTaskState(this TaskData taskData)
        {
            return TaskManager.Instance.GetTaskState(taskData.Taskid);
        }
    }
    
    public class TaskManager : GamePlaySystemBase<TaskManager>
    {
        private Dictionary<int, PlayerTaskData> _playerTaskData;
        public override void OnAwake()
        {
            base.OnAwake();
            EventCenter.AddListener<LoadDataEvent>(InitData);
            EventCenter.AddListener<InitializeSaveDataEvent>(InitializeTask);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadDataEvent>(InitData);
            EventCenter.RemoveListener<InitializeSaveDataEvent>(InitializeTask);
            base.OnDestroyed();
        }

        private void InitData(LoadDataEvent evt)
        {
            Managers.Get<ISaveDataManager>().GetPlayerTasks(ref _playerTaskData);
            EventCenter.DispatchEvent(new RefreshTaskEvent
            {
                playerTask = GetPlayerTask()
            });
        }
        
        private void InitializeTask(InitializeSaveDataEvent evt)
        {
            Managers.Get<ISaveDataManager>().GetPlayerTasks(ref _playerTaskData);
            var beginnerTask = ConfigTable.Instance.GetTasks().FindAll(item => item.Previousid == -1);
            foreach (var task in beginnerTask)
            {
                ActivateTask(task.Taskid);
            }
        }

        public void TriggerTask(TaskType taskType, int targetID, int targetNum = 1)
        {
            var runningTasks = ConfigTable.Instance.GetTasks().FindAll(item =>
                item.GetTaskState() == TaskState.Accepted && item.Tasktype == (int)taskType && item.Targetid.Contains(targetID));
            bool hasTaskFinished = false;
            foreach (var task in runningTasks)
            {
                SetTaskNum(task.Taskid, targetID, targetNum);
                
                if (IsTaskFinished(task.Taskid))
                {
                    hasTaskFinished = true;
                    ChangeTaskState(task.Taskid, TaskState.Finished);
                }
            }

            if (hasTaskFinished)
            {
                EventCenter.DispatchEvent(new RefreshTaskEvent
                {
                    playerTask = GetPlayerTask()
                });
            }
        }

        public void GetReward(int taskID)
        {
            ChangeTaskState(taskID, TaskState.Rewarded);
            Debug.LogError($"已领取ID为{taskID}的奖励");
            
            var nextTasks = ConfigTable.Instance.GetTasks().FindAll(item => item.Previousid == taskID);
            foreach (var task in nextTasks)
            {
                ActivateTask(task.Taskid);
            }
            
            EventCenter.DispatchEvent(new RefreshTaskEvent
            {
                playerTask = GetPlayerTask()
            });
        }
        
        private Dictionary<int, PlayerTaskData> GetPlayerTask()
        {
            Dictionary<int, PlayerTaskData> data = new();
            foreach (var task in _playerTaskData.Where(
                         task => task.Value.state is TaskState.Accepted or TaskState.Finished))
            {
                data[task.Key] = task.Value;
            }

            return data;
        }
        
        private void ActivateTask(int id)
        {
            var itemCount = ConfigTable.Instance.GetTask(id).Targetid.Length;
            _playerTaskData[id] = new PlayerTaskData
            {
                state = TaskState.Accepted,
                currentNum = new int[itemCount],
            };
            Debug.LogWarning($"已开启id为{id}的任务");
            Managers.Get<ISaveDataManager>().SaveData();
            
        }

        public TaskState GetTaskState(int id)
        {
            return _playerTaskData.ContainsKey(id) ? _playerTaskData[id].state : TaskState.Error;
        }
        
        private void ChangeTaskState(int id, TaskState state)
        {
            if (_playerTaskData.ContainsKey(id))
            {
                _playerTaskData[id].state = state;
            }
        }
        
        private void SetTaskNum(int taskID, int itemID, int num)
        {
            if (_playerTaskData.ContainsKey(taskID))
            {
                var taskData = ConfigTable.Instance.GetTask(taskID);
                var index = Array.IndexOf(taskData.Targetid, itemID);

                if (index < 0) return;
                _playerTaskData[taskID].currentNum[index] += num;
                
            }
        }

        private bool IsTaskFinished(int taskID)
        {
            var taskData = ConfigTable.Instance.GetTask(taskID);

            for (var i = 0; i < taskData.Targetid.Length; i++)
            {
                if (_playerTaskData[taskID].currentNum[i] < taskData.Targetnum[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}