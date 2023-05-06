using Game.UI.Component;
using Game.UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Shop
{
    public class ItemSellPanel_Nodes : MonoBehaviour
    {
        public Button close_btn;
        public ListComponent bag_list;

        private void Awake()
        {
            bag_list.Init();
        }
    }
}