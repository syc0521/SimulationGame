using Game.UI.Component;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Bag
{
    public class BagPanel_Nodes : MonoBehaviour
    {
        public Button close_btn;
        public ListComponent building_list;

        private void Awake()
        {
            building_list.Init();
        }
    }
}