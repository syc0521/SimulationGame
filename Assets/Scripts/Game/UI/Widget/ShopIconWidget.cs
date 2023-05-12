using System;
using Game.Audio;
using Game.Core;
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
        public TextMeshProUGUI name_txt, count_txt;
        public ListComponent currency_list;
        private int id;
        private Action<int> _clickHandler;

        public override void OnDestroyed()
        {
            _clickHandler = null;
            icon_img.OnDestroyed();
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
            if (shopListData.itemCount > 1)
            {
                count_txt.text = shopListData.itemCount.ToString();
            }

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
            Managers.Get<IAudioManager>().PlaySFX(SFXType.Button2);
        }
    }
}