using System;
using Game.UI.Component;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Building
{
    public class GovernmentPanel_Nodes : MonoBehaviour
    {
        public TextMeshProUGUI level_txt;
        public TextMeshProUGUI cityState_txt, cityEnv_txt;
        public TextMeshProUGUI currentLevel_txt, nextLevel_txt;
        public Button achievement_btn, upgrade_btn;
        public FrameComponent upgrade_frame;
        public Button back_btn;
        public ListComponent upgradeItem_list;
        public GameObject achievement_go;

        private void Awake()
        {
            upgradeItem_list.Init();
        }
    }
}