using Game.GamePlaySystem.Setting;
using Game.UI.Decorator;

namespace Game.UI.Panel.Pause
{
    public class PausePanel : UIPanel
    {
        public PausePanel_Nodes nodes;
        private float _bgmVolume, _soundVolume;
        
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.reset_btn.onClick.AddListener(ResetSave);
            nodes.quit_btn.onClick.AddListener(QuitGame);
            nodes.return_btn.onClick.AddListener(ReturnToGame);
            nodes.bgm_slider.SetHandler(SetBGMVolume);
            nodes.sound_slider.SetHandler(SetSoundVolume);
        }

        public override void OnShown()
        {
            base.OnShown();
            _bgmVolume = SettingManager.Instance.BGMVolume;
            _soundVolume = SettingManager.Instance.SoundVolume;
            nodes.bgm_slider.SetData(_bgmVolume);
            nodes.sound_slider.SetData(_soundVolume);
        }

        public override void OnDestroyed()
        {
            nodes.reset_btn.onClick.RemoveListener(ResetSave);
            nodes.quit_btn.onClick.RemoveListener(QuitGame);
            nodes.return_btn.onClick.RemoveListener(ReturnToGame);
            nodes.bgm_slider.RemoveHandler();
            nodes.sound_slider.RemoveHandler();
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

        private void SetBGMVolume(float f)
        {
            _bgmVolume = f;
        }

        private void SetSoundVolume(float f)
        {
            _soundVolume = f;
        }

        private void ReturnToGame()
        {
            SettingManager.Instance.SetVolume(_bgmVolume, _soundVolume);
        }
    }
}