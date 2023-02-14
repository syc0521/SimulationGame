using Game.UI.Component;
using Game.UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Common
{
    public class AlertRewardPanel_Nodes : MonoBehaviour
    {
        public Button confirm_btn, closeTip_btn;
        public ListComponent reward_list;
        public RectTransform tip_go;
        public TipWidget tip_w;
        
        private void Awake()
        {
            reward_list.Init();
        }
    }
}