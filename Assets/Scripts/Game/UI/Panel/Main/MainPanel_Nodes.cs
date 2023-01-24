using System;
using Game.UI.Component;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.UI.Panel
{
    public class MainPanel_Nodes : MonoBehaviour
    {
        public Button build_btn, destroy_btn;
        public TextMeshProUGUI people_txt, money_txt;
        public ListComponent task_list;

        private void Awake()
        {
            task_list.Init();
        }
    }
}