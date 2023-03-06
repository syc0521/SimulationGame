using Game.Core;
using Game.Data.Event.Common;
using Game.GamePlaySystem.Loading;

namespace Game.UI.Panel.Loading
{
    public class LoadingPanel : UIPanel
    {
        public LoadingPanel_Nodes nodes;
        public override void OnCreated()
        {
            base.OnCreated();
            EventCenter.AddListener<LoadingEvent>(ShowLoadingProgress);
        }

        public override void OnShown()
        {
            base.OnShown();
            LoadGame();
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadingEvent>(ShowLoadingProgress);
            base.OnDestroyed();
        }

        private void LoadGame()
        {
            LoadingManager.Instance.StartLoadingGame();
        }

        private void ShowLoadingProgress(LoadingEvent evt)
        {
            nodes.slider.SetValue(evt.progress);
            nodes.progress_txt.text = evt.text;
        }
    }
}