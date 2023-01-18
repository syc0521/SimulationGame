using Game.Core;
using Game.Data;
using Game.Data.Event.Task;
using Game.UI.UISystem;

namespace Game.UI.Module
{
    public class TaskModule : BaseModule
    {
        public override void OnCreated()
        {
            //EventCenter.AddListener<RefreshTaskEvent>(RefreshTask);
        }

        public override void OnDestroyed()
        {
            //EventCenter.RemoveListener<RefreshTaskEvent>(RefreshTask);
        }

        private void RefreshTask(RefreshTaskEvent evt)
        {
            //TaskSystem.Instance.GetTaskData();
        }
    }
}