using Game.Audio;
using Game.Core;
using Game.Data.Event.Audio;
using Game.Data.Event.Common;
using Game.Data.Event.FeatureOpen;
using Game.Input;
using Game.UI.Panel;
using Game.UI.Panel.FeatureOpen;
using Game.UI.Panel.GM;
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
            EventCenter.AddListener<OpenGMEvent>(ShowGMPanel);
        }

        public override void OnStart()
        {
            base.OnStart();
            PlayTitleBGM();
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadSceneFinishedEvent>(ShowMainPanel);
            EventCenter.RemoveListener<UnlockFeatureEvent>(ShowFeatureOpenPanel);
            EventCenter.RemoveListener<OpenGMEvent>(ShowGMPanel);
        }

        private void ShowMainPanel(LoadSceneFinishedEvent evt)
        {
            UIManager.Instance.ClosePanel<LoadingPanel>();
            UIManager.Instance.OpenPanel<MainPanel>();
            Managers.Get<IAudioManager>().PlayBGM(BGMType.GamePlay);
        }

        private void ShowFeatureOpenPanel(UnlockFeatureEvent evt)
        {
            UIManager.Instance.OpenPanel<FeatureOpenPanel>(new FeatureOpenPanelOption
            {
                type = evt.type
            });
        }

        private void PlayTitleBGM()
        {
            Managers.Get<IAudioManager>().PlayBGM(BGMType.Title, 0.8f);
        }
        
        private void ShowGMPanel(OpenGMEvent evt)
        {
            UIManager.Instance.OpenPanel<GMPanel>();
        }

    }
}