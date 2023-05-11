
using System.Collections.Generic;
using Game.Data;
using Game.GamePlaySystem.Task;
using Game.UI.Component;
using Game.UI.Module;
using Game.UI.Panel.Common;
using Game.UI.UISystem;

namespace Game.UI.Panel.Task
{
    public class TaskDetailPanelOption : BasePanelOption
    {
        public int taskID;
    }

    public class TaskDetailData : ListData
    {
        public string name;
        public int current, amount;
    }

    public class TaskDetailPanel : UIPanel
    {
        public TaskDetailPanel_Nodes nodes;
        private int taskID;
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.close_btn.onClick.AddListener(ClosePanel);
            nodes.claim_btn.onClick.AddListener(ClaimReward);
        }

        public override void OnShown()
        {
            base.OnShown();
            PlayAnimation();
            InitData();
        }

        public override void OnDestroyed()
        {
            nodes.close_btn.onClick.RemoveListener(ClosePanel);
            nodes.claim_btn.onClick.RemoveListener(ClaimReward);
        }

        private void ClosePanel()
        {
            CloseSelf();
        }

        private void InitData()
        {
            if (opt is not TaskDetailPanelOption option)
            {
                return;
            }
            
            taskID = option.taskID;
            var taskData = TaskSystem.Instance.GetTaskData(taskID);
            nodes.task_txt.text = taskData.name;
            nodes.detail_txt.text = taskData.content;
            nodes.claim_btn.gameObject.SetActive(taskData.state is TaskState.Finished);
            for (int i = 0; i < taskData.targetID.Length; i++)
            {
                nodes.completion_list.AddItem(new TaskDetailData
                {
                    name = taskData.type.GetTaskTargetName(taskData.targetID[i]),
                    current = taskData.currentNum[i] > taskData.targetNum[i] ? taskData.targetNum[i] : taskData.currentNum[i],
                    amount = taskData.targetNum[i],
                });
            }
        }

        private void ClaimReward()
        {
            var reward = TaskSystem.Instance.GetTaskData(taskID).reward;
            TaskManager.Instance.GetReward(taskID);
            if (reward is { Count: > 0 })
            {
                UIManager.Instance.OpenPanel<AlertRewardPanel>(new AlertRewardPanelOption
                {
                    data = new List<RewardData>(reward),
                    clickHandler = CloseSelf
                });
            }
            else
            {
                CloseSelf();
            }
        }
    }
}