﻿using Game.Audio;
using Game.Core;
using Game.GamePlaySystem;
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
            nodes.snap_btn.onClick.AddListener(SnapPhoto);
            nodes.return_btn.onClick.AddListener(ReturnToGame);
            nodes.bgm_slider.SetHandler(SetBGMVolume);
            nodes.sound_slider.SetHandler(SetSoundVolume);
        }

        public override void OnShown()
        {
            base.OnShown();
            PlayAnimation();
            _bgmVolume = SettingManager.Instance.BGMVolume;
            _soundVolume = SettingManager.Instance.SoundVolume;
            nodes.bgm_slider.SetValue(_bgmVolume);
            nodes.sound_slider.SetValue(_soundVolume);
        }

        public override void OnDestroyed()
        {
            nodes.reset_btn.onClick.RemoveListener(ResetSave);
            nodes.quit_btn.onClick.RemoveListener(QuitGame);
            nodes.snap_btn.onClick.RemoveListener(SnapPhoto);
            nodes.return_btn.onClick.RemoveListener(ReturnToGame);
            nodes.bgm_slider.RemoveHandler();
            nodes.sound_slider.RemoveHandler();
            base.OnDestroyed();
        }

        private void ResetSave()
        {
            AlertDecorator.OpenAlertPanel("是否重置存档？(需重启游戏)", true, () =>
            {
                SettingManager.Instance.ResetSaveData();
            });
        }

        private void QuitGame()
        {
            AlertDecorator.OpenAlertPanel("是否退出游戏？", true, () =>
            {
                SettingManager.Instance.SetVolume(_bgmVolume, _soundVolume);
                SettingManager.Instance.QuitGame();
            });
        }

        private void SnapPhoto()
        {
            CameraManager.Instance.TakePhoto();
        }

        private void SetBGMVolume(float f)
        {
            _bgmVolume = f;
            Managers.Get<IAudioManager>().AdjustBGMVolume(f);
        }

        private void SetSoundVolume(float f)
        {
            _soundVolume = f;
            Managers.Get<IAudioManager>().AdjustSoundVolume(f);
        }

        private void ReturnToGame()
        {
            Managers.Get<IAudioManager>().PlaySFX(SFXType.Button2);
            SettingManager.Instance.SetVolume(_bgmVolume, _soundVolume);
            CloseSelf();
        }
    }
}