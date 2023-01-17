
using Game.Data;
using Game.UI.Module;

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
            
            InitData();
        }

        public override void OnDestroyed()
        {
            nodes.close_btn.onClick.RemoveListener(ClosePanel);
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
            nodes.task_txt.text =
                $"{taskID} {ConfigTable.Instance.GetTasks().Find(item => item.Taskid == taskID).Name}";
        }
    }
}