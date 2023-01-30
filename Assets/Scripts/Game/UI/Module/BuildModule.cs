using Game.Core;
using Game.Data.Event;
using Game.UI.Decorator;
using Game.UI.Panel;

namespace Game.UI.Module
{
    public class BuildModule : BaseModule
    {
        public override void OnCreated()
        {
            EventCenter.AddListener<BuildUIEvent>(ShowConfirmUI);
            EventCenter.AddListener<DestroyEvent>(ShowDestroyAlert);
        }
        
        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<BuildUIEvent>(ShowConfirmUI);
        }

        private void ShowConfirmUI(BuildUIEvent obj)
        {
            if (!UIManager.Instance.HasPanel<ConfirmPanel>())
            {
                UIManager.Instance.OpenPanel<ConfirmPanel>();
            }
        }

        private void ShowDestroyAlert(DestroyEvent evt)
        {
            AlertDecorator.OpenAlertPanel("是否删除该建筑", evt.handler);
        }
        
    }
}