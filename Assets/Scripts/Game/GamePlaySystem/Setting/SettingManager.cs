using Game.Core;
using Game.Data;
using Game.Data.Event;
using UnityEditor;
using UnityEngine;

namespace Game.GamePlaySystem.Setting
{
    public class SettingManager : GamePlaySystemBase<SettingManager>
    {
        private SettingData _settingData;
        public float BGMVolume => _settingData.bgmVolume;
        public float SoundVolume => _settingData.soundVolume;
        public override void OnAwake()
        {
            base.OnAwake();
            EventCenter.AddListener<LoadDataEvent>(InitData);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadDataEvent>(InitData);
            base.OnDestroyed();
        }

        private void InitData(LoadDataEvent evt)
        {
            Managers.Get<ISaveDataManager>().GetSettingData(ref _settingData);
        }

        public void ResetSaveData()
        {
            Managers.Get<ISaveDataManager>().ResetSaveData();
        }

        public void QuitGame()
        {
            Managers.Get<ISaveDataManager>().SaveData();
            
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else   
            Application.Quit();
#endif
        }

        public void SetVolume(float bgm, float sound)
        {
            _settingData.bgmVolume = bgm;
            _settingData.soundVolume = sound;
            Managers.Get<ISaveDataManager>().SaveData();
        }

    }
}