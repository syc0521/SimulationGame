using Game.Data;
using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Shop;
using Game.UI.Utils;

namespace Game.UI.Panel.Shop
{
    public class SellDetailPanelOption : BasePanelOption
    {
        public int itemID;
    }
    
    public class SellDetailPanel : UIPanel
    {
        public SellDetailPanel_Nodes nodes;
        private int _itemID;
        private int _currentCount = 1, _totalCount, _price;
        private float _sellRate;
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.add_btn.onClick.AddListener(Add);
            nodes.reduce_btn.onClick.AddListener(Reduce);
            nodes.sell_btn.onClick.AddListener(Sell);
            nodes.amount_input.onValueChanged.AddListener(ChangeCount);
        }

        public override void OnShown()
        {
            base.OnShown();
            PlayAnimation();
            InitData();
            ShowItem();
        }

        public override void OnDestroyed()
        {
            nodes.add_btn.onClick.RemoveListener(Add);
            nodes.reduce_btn.onClick.RemoveListener(Reduce);
            nodes.sell_btn.onClick.RemoveListener(Sell);
            nodes.amount_input.onValueChanged.RemoveListener(ChangeCount);
            nodes.icon_img.OnDestroyed();
            base.OnDestroyed();
        }

        private void InitData()
        {
            if (opt is not SellDetailPanelOption option) return;
            _itemID = option.itemID;
            _sellRate = ShopManager.Instance.GetSellRate();
            _totalCount = BackpackManager.Instance.GetBackpackCount(_itemID);
        }

        private void ShowItem()
        {
            var itemData = ConfigTable.Instance.GetBagItemData(_itemID);
            _price = itemData.Price;
            nodes.title_txt.text = itemData.Name;
            nodes.content_txt.text = itemData.Content;
            nodes.icon_img.SetIcon(IconUtility.GetItemIcon(_itemID));
        }

        private void Sell()
        {
            ShopManager.Instance.SellItem(_itemID, _currentCount);
            CloseSelf();
        }

        private void Reduce()
        {
            if (_currentCount <= 1)
            {
                _currentCount = 1;
            }
            else
            {
                _currentCount--;
            }

            Refresh();
        }

        private void Add()
        {
            if (_currentCount >= _totalCount)
            {
                _currentCount = _totalCount;
            }
            else
            {
                _currentCount++;
            }

            Refresh();
        }

        private void ChangeCount(string text)
        {
            var count = int.Parse(text);
            if (count <= 1)
            {
                count = 1;
            }
            else if (count > _totalCount)
            {
                count = _totalCount;
            }

            _currentCount = count;
            Refresh();
        }

        private void Refresh()
        {
            nodes.amount_input.text = _currentCount.ToString();
            var amount = (int)(_price * _currentCount * _sellRate);
            nodes.currency_w.SetAmount(amount);
        }
    }
}