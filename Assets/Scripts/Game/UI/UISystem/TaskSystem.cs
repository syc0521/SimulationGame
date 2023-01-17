using System.Collections.Generic;
using Game.UI.ViewData;

namespace Game.UI.UISystem
{
    public class TaskSystem : UISystemBase
    {
        private Dictionary<int, TaskViewData> _taskData = new();

        public override void OnAwake()
        {
            base.OnAwake();
        }

        public override void OnDestroyed()
        {
            base.OnDestroyed();
        }
    }
}