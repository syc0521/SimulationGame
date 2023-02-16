using System;
using System.Collections.Generic;
using Game.UI.Component;
using Game.UI.UISystem;
using Game.UI.ViewData;
using Game.UI.Widget;
using UnityEngine;

namespace Game.UI.Panel.Bag
{
    public class BagListData : ListData
    {
        public int id;
        public BackpackViewData data;
        public Action<BagItemWidget, int> clickHandler;
    }
    public class BagPanel : UIPanel
    {
        private Dictionary<int, BackpackViewData> _backpack;
        public BagPanel_Nodes nodes;
        
        public override void OnCreated()
        {
            nodes.close_btn.onClick.AddListener(ClosePanel);
            nodes.closeTip_btn.onClick.AddListener(CloseTip);
        }
        
        public override void OnShown()
        {
            base.OnShown();
            InitData();
            InitBagList();
        }
        
        public override void OnDestroyed()
        {
            nodes.close_btn.onClick.RemoveListener(ClosePanel);
            nodes.closeTip_btn.onClick.RemoveListener(CloseTip);
            base.OnDestroyed();
        }
        
        private void ClosePanel()
        {
            CloseSelf();
        }

        private void InitData()
        {
            _backpack = BackpackSystem.Instance.GetBackpackData();
        }

        private void InitBagList()
        {
            nodes.bag_list.Clear();
            foreach (var item in _backpack)
            {
                nodes.bag_list.AddItem(new BagListData
                {
                    id = item.Key,
                    data = item.Value,
                    clickHandler = ClickBagItem
                });
            }
        }

        private void ClickBagItem(BagItemWidget widget, int index)
        {
            nodes.tip_go.gameObject.SetActive(true);
            nodes.tip_w.SetTitle(_backpack[index].name);
            nodes.tip_w.SetDescription(_backpack[index].description);
            var heightRatio = Screen.height / 1080.0f;
            nodes.tip_go.position = widget.transform.position + Vector3.down * 140 * heightRatio;
        }

        private void CloseTip()
        {
            nodes.tip_go.gameObject.SetActive(false);
        }
    }
}