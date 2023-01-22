using System;
using Game.UI.Component;
using Game.UI.Panel.Building;
using TMPro;
using UnityEngine.EventSystems;

namespace Game.UI.Widget
{
    public class BuildingIconWidget : WidgetBase, IListWidget, IPointerClickHandler
    {
        public CustomImage icon_img;
        public TextMeshProUGUI name_txt;
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

            id = buildingListData.id;
            _clickHandler = buildingListData.clickHandler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickHandler?.Invoke(id);
        }
    }
}