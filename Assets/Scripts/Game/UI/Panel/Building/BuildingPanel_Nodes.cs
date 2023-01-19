using Game.UI.Component;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Building
{
    public class BuildingPanel_Nodes : MonoBehaviour
    {
        public Button close_btn;
        public ListComponent building_list;

        private void Awake()
        {
            building_list.Init();
        }
    }
}