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
            LoadGame();
        }

        public override void OnDestroyed()
        {
            base.OnDestroyed();
        }

        private void LoadGame()
        {
            LoadingManager.Instance.StartLoadingGame();
        }
    }
}