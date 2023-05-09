using Game.UI.Component;
using TMPro;
using UnityEngine.UI;

namespace Game.UI.Panel.Task
{
    public class DailyTaskPanel_Nodes
    {
        public ListComponent task_list;
        public Button back_btn;
        
        private void Awake()
        {
            task_list.Init();
        }
    }
}