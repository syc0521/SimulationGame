using Game.Audio;
using Game.Core;
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
        }

        public override void OnDestroyed()
        {
            base.OnDestroyed();
            nodes.start_btn.onClick.RemoveListener(StartGame);
        }

        private void StartGame()
        {
            CloseSelf();
            UIManager.Instance.OpenPanel<LoadingPanel>();
        }
    }
}