using Game.Core;
using Game.Data.Event.Common;
using Game.Data.Event.FeatureOpen;
using Game.Input;
using Game.UI.Panel;
using Game.UI.Panel.FeatureOpen;
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
            EventCenter.AddListener<UnlockFeatureEvent>(ShowFeatureOpenPanel);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadSceneFinishedEvent>(ShowMainPanel);
            EventCenter.RemoveListener<UnlockFeatureEvent>(ShowFeatureOpenPanel);
        }

        private void ShowMainPanel(LoadSceneFinishedEvent evt)
        {
            UIManager.Instance.ClosePanel<LoadingPanel>();
            UIManager.Instance.OpenPanel<MainPanel>();
        }

        private void ShowFeatureOpenPanel(UnlockFeatureEvent evt)
        {
            UIManager.Instance.OpenPanel<FeatureOpenPanel>(new FeatureOpenPanelOption
            {
                type = evt.type
            });
        }

    }
}