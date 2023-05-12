using System;
using Game.Audio;
using Game.Core;
using Game.UI.Component;
using Game.UI.Panel.Bag;
using Game.UI.Panel.Common;
using Game.UI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Widget
{
    public class BagItemWidget : WidgetBase, IListWidget, IPointerClickHandler
    {
        public TextMeshProUGUI count_txt;
        public CustomImage icon_img;
        public GameObject count_go;

        private int id;
        private Action<BagItemWidget, int> _clickHandler;

        public override void OnDestroyed()
        {
            icon_img.OnDestroyed();
            base.OnDestroyed();
        }

        public void Refresh(ListData data)
        {
            if (data is BagListData bagListData)
            {
                id = bagListData.id;
                var count = bagListData.data.count;
                icon_img.SetIcon(IconUtility.GetItemIcon(bagListData.id));
                count_txt.text = count > 1 ? count.ToString() : string.Empty;
                count_go.SetActive(count > 1);
                _clickHandler = bagListData.clickHandler; 
            }
            else if (data is AlertRewardListData alertRewardListData)
            {
                id = alertRewardListData.id;
                var count = alertRewardListData.data.amount;
                count_txt.text = count > 1 ? count.ToString() : string.Empty;
                count_go.SetActive(count > 1);
                _clickHandler = alertRewardListData.clickHandler;
                icon_img.SetIcon(IconUtility.GetRewardIcon(alertRewardListData.data.type, alertRewardListData.data.itemID));
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clickHandler.Invoke(this, id);
            Managers.Get<IAudioManager>().PlaySFX(SFXType.Button2);
        }
        
    }
}