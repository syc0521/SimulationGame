using System;
using Game.UI.Component;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Task
{
    public class TaskDetailPanel_Nodes : MonoBehaviour
    {
        public Button close_btn, claim_btn;
        public TextMeshProUGUI task_txt, detail_txt;
        public ListComponent completion_list;

        private void Awake()
        {
            completion_list.Init();
        }
    }
}