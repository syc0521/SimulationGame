using System;
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
        [SerializeField]
        public TextMeshProUGUI count_txt;

        private int id;
        private Action<BagItemWidget, int> _clickHandler;
        public void Refresh(ListData data)
        {
            if (data is BagListData bagListData)
            {
                id = bagListData.id;
                var count = bagListData.data.count;
                count_txt.text = count > 1 ? count.ToString() : string.Empty;
                _clickHandler = bagListData.clickHandler; 
            }
            else if (data is AlertRewardListData alertRewardListData)
            {
                id = alertRewardListData.id;
                var count = alertRewardListData.data.amount;
                count_txt.text = count > 1 ? count.ToString() : string.Empty;
                _clickHandler = alertRewardListData.clickHandler; 
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickHandler.Invoke(this, id);
        }
    }
}