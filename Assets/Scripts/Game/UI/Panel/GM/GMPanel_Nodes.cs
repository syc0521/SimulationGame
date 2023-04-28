using System;
using Game.UI.Component;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.GM
{
    public class GMPanel_Nodes : MonoBehaviour
    {
        public Button close_btn;
        public ListComponent name_list;
        public ListComponent type_list;
        public ListComponent info_list;
        public Button run_btn;

        private void Awake()
        {
            name_list.Init();
            type_list.Init();
            info_list.Init();
        }
    }
}