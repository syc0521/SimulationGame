using System;
using Game.UI.Component;
using Game.UI.Widget;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI.Panel
{
    public class MainPanel_Nodes : MonoBehaviour
    {
        public Button build_btn, destroy_btn, bag_btn;
        public ListComponent task_list;
        public OperateBuildingWidget operate_widget;
        public Button pause_btn;
        public StatusWidget happiness_widget, people_widget, money_widget;
        public TipWidget tip_w;
        public Button closeTip_btn;

        private void Awake()
        {
            task_list.Init();
        }
    }
}