using System;
using Game.UI.Component;
using Game.UI.Panel.Building;
using Game.UI.Utils;
using Game.UI.ViewData;
using TMPro;
using UnityEngine.EventSystems;

namespace Game.UI.Widget
{
    public class ConsumeItemListData : ListData
    {
        public ConsumeType consumeType;
        public int id;
        public int amount;
    }
    
    public class BuildingIconWidget : WidgetBase, IListWidget, IPointerClickHandler
    {
        public CustomImage icon_img;
        public TextMeshProUGUI name_txt;
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
            if (data is not BuildingListData buildingListData)
            {
                return;
            }
            
            icon_img.SetIcon(IconUtility.GetBuildingIcon(buildingListData.id));
            name_txt.text = buildingListData.data.name;

            if (buildingListData.id != 4) //不是道路
            {
                int itemLength = buildingListData.data.itemType.Length;
                currency_list.Init();
                for (int i = 0; i < itemLength; i++)
                {
                    currency_list.AddItem(new ConsumeItemListData
                    {
                        consumeType = ConsumeType.Item,
                        id = buildingListData.data.itemType[i],
                        amount = buildingListData.data.itemCount[i]
                    });
                }
            }
            
            id = buildingListData.id;
            _clickHandler = buildingListData.clickHandler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickHandler?.Invoke(id);
        }
    }
}