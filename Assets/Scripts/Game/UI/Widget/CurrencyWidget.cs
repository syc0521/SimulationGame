using Game.GamePlaySystem.Backpack;
using Game.UI.Component;
using Game.UI.Utils;
using Game.UI.ViewData;
using TMPro;
using UnityEngine;

namespace Game.UI.Widget
{
    public class CurrencyWidget : WidgetBase, IListWidget
    {
        public CustomImage icon;
        public TextMeshProUGUI price;
        public void Refresh(ListData data)
        {
            if (data is ConsumeItemListData itemListData)
            {
                var type = itemListData.consumeType;
                icon.SetIcon(type is ConsumeType.Currency ? IconUtility.GetCurrencyIcon(itemListData.id)
                    : IconUtility.GetItemIcon(itemListData.id));
                price.text = itemListData.amount.ToString();
                price.color = BackpackManager.Instance.GetBackpackCount(itemListData.id) >= itemListData.amount ? Color.white : Color.red;
            }
        }
    }
}