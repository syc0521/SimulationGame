using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Common;
using Game.Data.Event;
using Game.Data.Event.Task;
using Game.Data.FeatureOpen;
using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.Currency;
using Game.GamePlaySystem.FeatureOpen;
using UnityEngine;

namespace Game.GamePlaySystem.Task
{
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
                if (taskType is TaskType.People or TaskType.GetEvaluateScore or TaskType.CountBuilding)
                {
                    SetTaskNum(task.Taskid, targetID, targetNum);
                }
                else
                {
                    AddTaskNum(task.Taskid, targetID, targetNum);
                }
                
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
            var taskData = ConfigTable.Instance.GetTask(taskID);
            var rewardGroup = ConfigTable.Instance.GetRewardGroupData(taskData.Reward);

            if (taskData.Featureopen != -1)
            {
                FeatureOpenManager.Instance.OpenFeature((FeatureType)taskData.Featureopen);
            }

            if (taskData.Reward != -1)
            {
                for (int i = 0; i < rewardGroup.Rewardtype.Length; i++)
                {
                    switch ((RewardType)rewardGroup.Rewardtype[i])
                    {
                        case RewardType.Currency:
                            CurrencyManager.Instance.AddCurrency((CurrencyType)rewardGroup.Itemid[i], rewardGroup.Count[i]);
                            break;
                        case RewardType.Building:
                            BuildingManager.Instance.UnlockBuilding(rewardGroup.Itemid[i]);
                            break;
                        case RewardType.Item:
                            BackpackManager.Instance.AddBackpackCount(rewardGroup.Itemid[i], rewardGroup.Count[i]);
                            break;
                    }
                }
            }
            
            Debug.LogWarning($"已领取ID为{taskID}的奖励");
            ChangeTaskState(taskID, TaskState.Rewarded);

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
        
        public Dictionary<int, PlayerTaskData> GetPlayerTask()
        {
            Dictionary<int, PlayerTaskData> data = new();
            foreach (var task in _playerTaskData.Where(
                         task => task.Value.state is TaskState.Accepted or TaskState.Finished))
            {
                data[task.Key] = task.Value;
            }

            return data;
        }
        
        public void ActivateTask(int id)
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
            return _playerTaskData.TryGetValue(id, out var value) ? value.state : TaskState.Error;
        }
        
        private void ChangeTaskState(int id, TaskState state)
        {
            if (_playerTaskData.TryGetValue(id, out var value))
            {
                value.state = state;
            }
        }
        
        private void AddTaskNum(int taskID, int itemID, int num)
        {
            if (_playerTaskData.TryGetValue(taskID, out var value))
            {
                var taskData = ConfigTable.Instance.GetTask(taskID);
                var index = Array.IndexOf(taskData.Targetid, itemID);

                if (index < 0) return;
                value.currentNum[index] += num;
            }
        }
        
        private void SetTaskNum(int taskID, int itemID, int num)
        {
            if (_playerTaskData.TryGetValue(taskID, out var value))
            {
                var taskData = ConfigTable.Instance.GetTask(taskID);
                var index = Array.IndexOf(taskData.Targetid, itemID);

                if (index < 0) return;
                value.currentNum[index] = num;
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