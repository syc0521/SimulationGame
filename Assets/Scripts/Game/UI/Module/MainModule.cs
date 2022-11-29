using Game.Core;
using Game.Input;
using Game.UI.Panel;

namespace Game.UI.Module
{
    public class MainModule : BaseModule
    {
        public override void OnCreated()
        {
            
        }

        public override void OnStart()
        {
            if (!UIManager.Instance.HasPanel<MainPanel>())
            {
                UIManager.Instance.CreatePanel<MainPanel>();
            }
        }

        public override void OnDestroyed()
        {
            
        }

    }
}