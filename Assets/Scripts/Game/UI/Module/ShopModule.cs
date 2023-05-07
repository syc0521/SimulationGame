using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.Data.Event.Shop;
using Game.UI.Panel.Common;

namespace Game.UI.Module
{
    public class ShopModule : BaseModule
    {
        public override void OnCreated()
        {
            EventCenter.AddListener<BuySuccessEvent>(OnBuySuccess);
            EventCenter.AddListener<SellEvent>(OnSellSuccess);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<BuySuccessEvent>(OnBuySuccess);
            EventCenter.RemoveListener<SellEvent>(OnSellSuccess);
        }

        private void OnBuySuccess(BuySuccessEvent evt)
        {
            UIManager.Instance.OpenPanel<AlertRewardPanel>(new AlertRewardPanelOption
            {
                data = new List<RewardData>{evt.RewardData},
            });
        }
        
        private void OnSellSuccess(SellEvent evt)
        {
            UIManager.Instance.OpenPanel<AlertRewardPanel>(new AlertRewardPanelOption
            {
                data = new List<RewardData>{evt.RewardData},
            });
        }
    }
}