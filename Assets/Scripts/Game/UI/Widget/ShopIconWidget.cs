using System;
using Game.UI.Component;
using Game.UI.Panel.Building;
using Game.UI.Panel.Shop;
using Game.UI.Utils;
using Game.UI.ViewData;
using TMPro;
using UnityEngine.EventSystems;

namespace Game.UI.Widget
{
    public class ShopIconWidget : WidgetBase, IListWidget, IPointerClickHandler
    {
        public CustomImage icon_img;
        public TextMeshProUGUI name_txt;
        public ListComponent currency_list;
        private int id;
        private Action<int> _clickHandler;

        public override void OnDestroyed()
        {
            _clickHandler = null;
            base.OnDestroyed();
        }

        public void Refresh(ListData data)
        {
            if (data is not ShopListData shopListData)
            {
                return;
            }
            
            icon_img.SetIcon(IconUtility.GetItemIcon(shopListData.staticId));
            name_txt.text = shopListData.name;

            int itemLength = shopListData.currencyType.Length;
            currency_list.Init();
            for (int i = 0; i < itemLength; i++)
            {
                currency_list.AddItem(new ConsumeItemListData
                {
                    consumeType = ConsumeType.Currency,
                    id = shopListData.currencyType[i],
                    amount = shopListData.currencyCount[i]
                });
            }
            
            id = shopListData.shopItemId;
            _clickHandler = shopListData.clickHandler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickHandler?.Invoke(id);
        }
    }
}