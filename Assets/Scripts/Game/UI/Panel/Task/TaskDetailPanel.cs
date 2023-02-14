
using System.Collections.Generic;
using Game.Data;
using Game.GamePlaySystem.Task;
using Game.UI.Module;
using Game.UI.Panel.Common;
using Game.UI.UISystem;

namespace Game.UI.Panel.Task
{
    public class TaskDetailPanelOption : BasePanelOption
    {
        public int taskID;
    }
    public class TaskDetailPanel : UIPanel
    {
        public TaskDetailPanel_Nodes nodes;
        private int taskID;
        public override void OnCreated()
        {
            nodes.close_btn.onClick.AddListener(ClosePanel);
            nodes.claim_btn.onClick.AddListener(ClaimReward);
        }

        public override void OnShown()
        {
            base.OnShown();
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
        }

        private void ClaimReward()
        {
            UIManager.Instance.OpenPanel<AlertRewardPanel>(new AlertRewardPanelOption
            {
                data = new List<RewardData>(TaskSystem.Instance.GetTaskData(taskID).reward),
                clickHandler = CloseSelf
            });
            TaskManager.Instance.GetReward(taskID);
        }
    }
}