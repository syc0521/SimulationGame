using System;
using Game.UI.Component;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Shop
{
    public class MallPanel_Nodes : MonoBehaviour
    {
        public CustomImage icon_img;
        public TextMeshProUGUI name_txt;
        public ListComponent currency_list;
        public Button back_btn, dailyItem_btn;

        private void Awake()
        {
            currency_list.Init();
        }
    }
}