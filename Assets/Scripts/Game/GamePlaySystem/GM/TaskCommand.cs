using Game.Core;
using Game.Data.Event.Task;
using Game.Data.GM;
using Game.GamePlaySystem.Task;

namespace Game.GamePlaySystem.GM
{
    [GMAttr(type = "任务", name = "接取任务", priority = 1)]
    public class AcceptTaskCommand : ICommand
    {
        public int taskId;
        public void Run()
        {
            TaskManager.Instance.ActivateTask(taskId);
            TaskManager.Instance.RefreshTask();
        }
    }
    
    [GMAttr(type = "任务", name = "完成任务", priority = 2)]
    public class FinishTaskCommand : ICommand
    {
        public int taskId;
        public void Run()
        {
            TaskManager.Instance.GetReward(taskId);
            TaskManager.Instance.RefreshTask();
        }
    }
}