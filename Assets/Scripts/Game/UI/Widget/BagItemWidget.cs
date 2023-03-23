using System;
using Game.Data;
using Game.UI.Component;
using Game.UI.Panel.Bag;
using Game.UI.Panel.Common;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Widget
{
    public class BagItemWidget : WidgetBase, IListWidget, IPointerClickHandler
    {
        public TextMeshProUGUI count_txt;
        public CustomImage icon_img;

        private int id;
        private Action<BagItemWidget, int> _clickHandler;
        public void Refresh(ListData data)
        {
            if (data is BagListData bagListData)
            {
                id = bagListData.id;
                var count = bagListData.data.count;
                icon_img.SetIcon(new AtlasSpriteID
                {
                    atlas = AtlasEnum.Item,
                    resName = $"icon_item_{bagListData.id}"
                });
                count_txt.text = count > 1 ? count.ToString() : string.Empty;
                _clickHandler = bagListData.clickHandler; 
            }
            else if (data is AlertRewardListData alertRewardListData)
            {
                id = alertRewardListData.id;
                var count = alertRewardListData.data.amount;
                count_txt.text = count > 1 ? count.ToString() : string.Empty;
                _clickHandler = alertRewardListData.clickHandler;
                icon_img.SetIcon(GetRewardIcon(alertRewardListData.data.type, alertRewardListData.data.itemID));
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickHandler.Invoke(this, id);
        }

        private AtlasSpriteID GetRewardIcon(RewardType type, int itemId)
        {
            return type switch
            {
                RewardType.Currency => new AtlasSpriteID
                {
                    atlas = AtlasEnum.Currency,
                    resName = $"icon_currency_{itemId}"
                },
                RewardType.Building => new AtlasSpriteID
                {
                    atlas = AtlasEnum.Building,
                    resName = $"icon_building_{itemId}"
                },
                RewardType.Item => new AtlasSpriteID
                {
                    atlas = AtlasEnum.Item,
                    resName = $"icon_item_{itemId}"
                },
                _ => default
            };
        }

    }
}