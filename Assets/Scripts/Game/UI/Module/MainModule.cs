using Game.Core;
using Game.Input;
using Game.UI.Panel;

namespace Game.UI.Module
{
    public class MainModule : BaseModule
    {
        public override void OnCreated()
        {
            UIManager.Instance.OpenPanel<MainPanel>();
        }

        public override void OnStart()
        {
            
        }

        public override void OnDestroyed()
        {
            
        }

    }
}