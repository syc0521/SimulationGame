using Game.Audio;
using Game.Core;
using Game.Data.Event.Common;
using Game.GamePlaySystem.Loading;
using Game.UI.Panel.Loading;

namespace Game.UI.Panel.Start
{
    public class StartPanel : UIPanel
    {
        public StartPanel_Nodes nodes;
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.start_btn.onClick.AddListener(StartGame);
            EventCenter.AddListener<LoadSceneFinishedEvent>(Close);
        }

        public override void OnDestroyed()
        {
            base.OnDestroyed();
            nodes.start_btn.onClick.RemoveListener(StartGame);
            EventCenter.AddListener<LoadSceneFinishedEvent>(Close);
        }

        private void StartGame()
        {
            UIManager.Instance.OpenPanel<LoadingPanel>();
        }

        private void Close(LoadSceneFinishedEvent evt)
        {
            CloseSelf();
        }
    }
}