using System;
using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Currency;
using Game.UI.Component;
using Game.UI.Panel.Building;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Widget
{
    public class BuildingIconWidget : WidgetBase, IListWidget, IPointerClickHandler
    {
        public CustomImage icon_img;
        public TextMeshProUGUI name_txt, desc_txt;
        public TextMeshProUGUI price_txt;
        private int id;
        private Action<int> _clickHandler;

        public override void OnDestroyed()
        {
            _clickHandler = null;
            base.OnDestroyed();
        }

        public void Refresh(ListData data)
        {
            if (data is not BuildingListData buildingListData)
            {
                return;
            }
            
            icon_img.SetIcon(new AtlasSpriteID
            {
                atlas = AtlasEnum.Building,
                resName = $"icon_building_{buildingListData.id}"
            });
            name_txt.text = buildingListData.data.name;
            desc_txt.text = buildingListData.data.description;
            var count = buildingListData.data.itemCount[0];
            var type = buildingListData.data.itemType[0];
            price_txt.text = count.ToString();
            price_txt.color = BackpackManager.Instance.GetBackpackCount(type) >= count ? Color.black : Color.red;

            id = buildingListData.id;
            _clickHandler = buildingListData.clickHandler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickHandler?.Invoke(id);
        }
    }
}