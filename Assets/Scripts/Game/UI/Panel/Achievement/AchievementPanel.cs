using Game.Data;
using Game.GamePlaySystem.Achievement;
using Game.GamePlaySystem.Task;
using Game.UI.Component;

namespace Game.UI.Panel.Achievement
{
    public class AchievementListData : ListData
    {
        public string name, desc;
        public int current, amount;
        public bool complete;
    }
    
    public class AchievementPanel : UIPanel
    {
        public AchievementPanel_Nodes nodes;

        public override void OnCreated()
        {
            base.OnCreated();
            nodes.back_btn.onClick.AddListener(CloseSelf);
        }

        public override void OnShown()
        {
            base.OnShown();
            PlayAnimation();
            RenderList();
            var currentNum = AchievementManager.Instance.GetCompletedAchievement;
            var totalNum = AchievementManager.Instance.GetTotalAchievement;
            nodes.completion_txt.text = $"已完成 {currentNum}/{totalNum}";
        }

        public override void OnDestroyed()
        {
            nodes.back_btn.onClick.RemoveListener(CloseSelf);
            base.OnDestroyed();
        }

        private void RenderList()
        {
            var achievementData = AchievementManager.Instance.GetAchievementData;
            foreach (var (id, data) in achievementData)
            {
                var tableData = ConfigTable.Instance.GetAchievementData(id);
                
                bool finishTask = true;
                if (tableData.Requiretask[0] != -1)
                {
                    foreach (var taskId in tableData.Requiretask)
                    {
                        if (TaskManager.Instance.GetTaskState(taskId) is not TaskState.Rewarded)
                        {
                            finishTask = false;
                        }
                    }
                }
                
                if ((tableData.Unlockcondition != -1 && !achievementData[tableData.Unlockcondition].complete) || !finishTask)
                {
                    continue;
                }
                
                nodes.achievement_list.AddItem(new AchievementListData
                {
                    name = tableData.Name,
                    desc = tableData.Content,
                    current = data.progress,
                    amount = tableData.Targetnum,
                    complete = data.complete,
                });
            }
        }
        
    }
}