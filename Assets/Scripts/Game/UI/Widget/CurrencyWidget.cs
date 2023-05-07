using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Currency;
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
                var hasEnough = type is ConsumeType.Currency
                    ? CurrencyManager.Instance.CheckCurrency(itemListData.id, itemListData.amount)
                    : BackpackManager.Instance.CheckBackpackItem(itemListData.id, itemListData.amount);
                price.color = hasEnough ? Color.white : Color.red;
            }
        }

        public void SetAmount(int amount)
        {
            price.text = amount.ToString();
        }
    }
}