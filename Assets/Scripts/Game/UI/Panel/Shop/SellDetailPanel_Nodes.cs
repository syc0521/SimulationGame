using Game.UI.Component;
using Game.UI.Widget;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel.Shop
{
    public class SellDetailPanel_Nodes : MonoBehaviour
    {
        public TextMeshProUGUI title_txt, content_txt;
        public CustomButton add_btn;
        public CustomButton reduce_btn;
        public CustomButton sell_btn;
        public CustomImage icon_img;
        public TMP_InputField amount_input;
        public CurrencyWidget currency_w;
        public Button close_btn;
    }
}