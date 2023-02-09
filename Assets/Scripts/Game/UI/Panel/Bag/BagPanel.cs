using System.Collections.Generic;
using Game.UI.UISystem;
using Game.UI.ViewData;

namespace Game.UI.Panel.Bag
{
    public class BagPanel : UIPanel
    {
        private Dictionary<int, BackpackViewData> _backpack;
        public BagPanel_Nodes nodes;
        
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
            
        }

        private void ClickBagItem()
        {
            
        }
    }
}