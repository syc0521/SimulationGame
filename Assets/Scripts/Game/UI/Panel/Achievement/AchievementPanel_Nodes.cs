using Game.UI.Component;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Achievement
{
    public class AchievementPanel_Nodes : MonoBehaviour
    {
        public TextMeshProUGUI completion_txt;
        public ListComponent achievement_list;
        public Button back_btn;
        
        private void Awake()
        {
            achievement_list.Init();
        }
    }
}