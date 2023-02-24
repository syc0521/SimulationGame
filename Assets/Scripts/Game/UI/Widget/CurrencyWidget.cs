using Game.GamePlaySystem.Backpack;
using Game.UI.Component;
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
            if (data is ConsumeItemListData currencyListData)
            {
                // todo 图标
                price.text = currencyListData.amount.ToString();
                price.color = BackpackManager.Instance.GetBackpackCount(currencyListData.type) >= currencyListData.amount ? Color.black : Color.red;
            }
        }
    }
}