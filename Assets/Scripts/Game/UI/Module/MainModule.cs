using Game.UI.Panel;

namespace Game.UI.Module
{
    public class MainModule : BaseModule
    {
        public override void OnCreated()
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