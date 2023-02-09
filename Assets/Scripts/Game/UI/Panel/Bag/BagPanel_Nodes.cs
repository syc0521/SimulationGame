using Game.UI.Component;
using Game.UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Bag
{
    public class BagPanel_Nodes : MonoBehaviour
    {
        public Button close_btn, closeTip_btn;
        public ListComponent bag_list;
        public RectTransform tip_go;
        public TipWidget tip_w;

        private void Awake()
        {
            bag_list.Init();
        }
    }
}