using Game.GamePlaySystem.Setting;
using Game.UI.Decorator;

namespace Game.UI.Panel.Pause
{
    public class PausePanel : UIPanel
    {
        public PausePanel_Nodes nodes;
        
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.reset_btn.onClick.AddListener(ResetSave);
            nodes.quit_btn.onClick.AddListener(QuitGame);
        }

        public override void OnDestroyed()
        {
            nodes.reset_btn.onClick.RemoveListener(ResetSave);
            nodes.quit_btn.onClick.RemoveListener(QuitGame);
            base.OnDestroyed();
        }

        private void ResetSave()
        {
            AlertDecorator.OpenAlertPanel("是否重置存档？", true, () =>
            {
                SettingManager.Instance.ResetSaveData();
            });
        }

        private void QuitGame()
        {
            AlertDecorator.OpenAlertPanel("是否退出游戏？", true, () =>
            {
                SettingManager.Instance.QuitGame();
            });
        }

        private void SnapPhoto()
        {
            // todo 截屏
        }
    }
}