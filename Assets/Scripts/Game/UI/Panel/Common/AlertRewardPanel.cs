using System;
using System.Collections.Generic;
using Game.Data;
using Game.UI.Component;
using Game.UI.Widget;
using UnityEngine;

namespace Game.UI.Panel.Common
{
    public class AlertRewardPanelOption : BasePanelOption
    {
        public List<RewardData> data;
        public Action clickHandler;
    }

    public class AlertRewardListData : ListData
    {
        public int id;
        public RewardData data;
        public Action<BagItemWidget, int> clickHandler;
    }
    
    public class AlertRewardPanel : UIPanel
    {
        public AlertRewardPanel_Nodes nodes;
        private Action _clickHandler;
        private List<RewardData> _rewardData = new(0);

        public override void OnCreated()
        {
            base.OnCreated();
            nodes.confirm_btn.onClick.AddListener(ClickConfirm);
            nodes.closeTip_btn.onClick.AddListener(CloseTip);
        }
        
        public override void OnShown()
        {
            PlayAnimation();
            if (opt is not AlertRewardPanelOption option) return;
            _clickHandler = option.clickHandler;
            _rewardData = option.data;
            InitRewardList();
        }
        
        public override void OnDestroyed()
        {
            nodes.confirm_btn.onClick.RemoveListener(ClickConfirm);
            nodes.closeTip_btn.onClick.RemoveListener(CloseTip);
            base.OnDestroyed();
        }
        
        private void CloseTip()
        {
            nodes.tip_go.gameObject.SetActive(false);
            nodes.closeTip_btn.gameObject.SetActive(false);
        }

        private void ClickConfirm()
        {
            CloseSelf();
            _clickHandler?.Invoke();
        }

        private void InitRewardList()
        {
            nodes.reward_list.Clear();
            for (var i = 0; i < _rewardData.Count; i++)
            {
                var item = _rewardData[i];
                nodes.reward_list.AddItem(new AlertRewardListData
                {
                    id = i,
                    data = item,
                    clickHandler = ClickBagItem,
                });
            }
        }
        
        private void ClickBagItem(BagItemWidget widget, int index)
        {
            nodes.tip_go.gameObject.SetActive(true);
            nodes.closeTip_btn.gameObject.SetActive(true);
            var data = _rewardData[index];
            switch (data.type)
            {
                case RewardType.Currency:
                    var currencyItem = ConfigTable.Instance.GetCurrencyData(data.itemID);
                    nodes.tip_w.SetTitle(currencyItem.Name);
                    nodes.tip_w.SetDescription(currencyItem.Content);
                    break;
                case RewardType.Building:
                    var buildingItem = ConfigTable.Instance.GetBuildingData(data.itemID);
                    nodes.tip_w.SetTitle(buildingItem.Name);
                    nodes.tip_w.SetDescription(buildingItem.Description);
                    break;
                case RewardType.Item:
                    var bagItem = ConfigTable.Instance.GetBagItemData(data.itemID);
                    nodes.tip_w.SetTitle(bagItem.Name);
                    nodes.tip_w.SetDescription(bagItem.Content);
                    break;
            }

            var heightRatio = Screen.height / 1080.0f;
            nodes.tip_go.position = widget.transform.position + Vector3.down * 140 * heightRatio;
        }

    }
}