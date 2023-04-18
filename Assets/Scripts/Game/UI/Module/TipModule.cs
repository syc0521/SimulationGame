using Game.Core;
using Game.Data.Event.Common;
using Game.UI.Decorator;

namespace Game.UI.Module
{
    public class TipModule : BaseModule
    {
        public override void OnCreated()
        {
            EventCenter.AddListener<AlertTipEvent>(OnAlertTip);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<AlertTipEvent>(OnAlertTip);
        }
        
        private void OnAlertTip(AlertTipEvent evt)
        {
            AlertDecorator.OpenAlertPanel(evt.tipText, false);
        }
    }
}