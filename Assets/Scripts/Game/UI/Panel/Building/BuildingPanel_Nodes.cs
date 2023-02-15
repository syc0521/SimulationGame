using Game.UI.Component;
using Game.UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Building
{
    public class BuildingPanel_Nodes : MonoBehaviour
    {
        public Button close_btn;
        public ListComponent building_list;
        public TabBar tabBar;

        private void Awake()
        {
            building_list.Init();
        }
    }
}