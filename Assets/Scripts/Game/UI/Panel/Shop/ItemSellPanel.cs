using System.Collections.Generic;
using Game.UI.Panel.Bag;
using Game.UI.UISystem;
using Game.UI.ViewData;
using Game.UI.Widget;

namespace Game.UI.Panel.Shop
{
    public class ItemSellPanel : UIPanel
    {
        private Dictionary<int, BackpackViewData> _backpack;
        public ItemSellPanel_Nodes nodes;
        
        public override void OnCreated()
        {
            nodes.close_btn.onClick.AddListener(ClosePanel);
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
            // todo 打开选择数量界面
        }

    }
}