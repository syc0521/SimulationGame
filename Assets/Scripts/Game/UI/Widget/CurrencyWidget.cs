using Game.GamePlaySystem.Backpack;
using Game.UI.Component;
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
                if (type is ConsumeType.Currency)
                {
                    icon.SetIcon(new AtlasSpriteID
                    {
                        atlas = AtlasEnum.Currency,
                        resName = $"icon_currency_{itemListData.id}"
                    });
                }
                else
                {
                    icon.SetIcon(new AtlasSpriteID
                    {
                        atlas = AtlasEnum.Item,
                        resName = $"icon_item_{itemListData.id}"
                    });
                }
                price.text = itemListData.amount.ToString();
                price.color = BackpackManager.Instance.GetBackpackCount(itemListData.id) >= itemListData.amount ? Color.white : Color.red;
            }
        }
    }
}