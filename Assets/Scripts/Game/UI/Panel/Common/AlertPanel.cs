using System;

namespace Game.UI.Panel.Common
{
    public class AlertPanelOption : BasePanelOption
    {
        public string content;
        public Action clickHandler;
    }
    
    public class AlertPanel : UIPanel
    {
        public AlertPanel_Nodes nodes;
        private Action _clickHandler;

        public override void OnCreated()
        {
            nodes.confirm_btn.onClick.AddListener(ClickConfirm);
            nodes.cancel_btn.onClick.AddListener(ClickCancel);
        }

        public override void OnShown()
        {
            if (opt is not AlertPanelOption option) return;

            nodes.detail_txt.text = option.content;
            _clickHandler = option.clickHandler;
        }

        public override void OnDestroyed()
        {
            nodes.confirm_btn.onClick.RemoveListener(ClickConfirm);
            nodes.cancel_btn.onClick.RemoveListener(ClickCancel);
        }

        private void ClickConfirm()
        {
            _clickHandler?.Invoke();
            CloseSelf();
        }

        private void ClickCancel()
        {
            CloseSelf();
        }
    }
}