using Game.Core;
using Game.Data.Event;
using Game.UI.Panel;

namespace Game.UI.Module
{
    public class BuildModule : BaseModule
    {
        public override void OnCreated()
        {
            EventCenter.AddListener<BuildUIEvent>(ShowConfirmUI);
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
        
    }
}