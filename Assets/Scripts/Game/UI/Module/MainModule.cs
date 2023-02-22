using Game.Core;
using Game.Data.Event.Common;
using Game.Input;
using Game.UI.Panel;
using Game.UI.Panel.Loading;
using Game.UI.Panel.Start;

namespace Game.UI.Module
{
    public class MainModule : BaseModule
    {
        public override void OnCreated()
        {
            UIManager.Instance.OpenPanel<StartPanel>();
            EventCenter.AddListener<LoadSceneFinishedEvent>(ShowMainPanel);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadSceneFinishedEvent>(ShowMainPanel);
        }

        private void ShowMainPanel(LoadSceneFinishedEvent evt)
        {
            UIManager.Instance.ClosePanel<LoadingPanel>();
            UIManager.Instance.OpenPanel<MainPanel>();
        }

    }
}