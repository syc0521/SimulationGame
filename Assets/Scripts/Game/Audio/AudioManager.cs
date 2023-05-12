using CriWare;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.Data.Event.Common;
using UnityEngine;

namespace Game.Audio
{
    public class AudioManager : ManagerBase, IAudioManager
    {
        public Transform AudioRoot => _audioRoot;
        public float SoundVolume => _soundVolume;
        public float BGMVolume => _bgmVolume;
        private Transform _audioRoot;
        private CriAtomSource _bgmPlayer, _ambientPlayer, _cityAmbPlayer;
        private const string Path = "Audio/PC";
        private float _bgmVolume = 0.8f, _soundVolume = 0.8f;
        private AudioPool _audioPool;
        
        public override void OnAwake()
        {
            base.OnAwake();
            _audioPool = new();
            _audioRoot = GameObject.Find("CRIWARE").transform;
            EventCenter.AddListener<LoadDataEvent>(InitializeAudio);
            EventCenter.AddListener<LoadSceneFinishedEvent>(PlayAmbientSound);

            _bgmPlayer = new GameObject("BGMPlayer").AddComponent<CriAtomSource>();
            _bgmPlayer.transform.SetParent(_audioRoot);
            _ambientPlayer = new GameObject("AmbientPlayer").AddComponent<CriAtomSource>();
            _ambientPlayer.transform.SetParent(_audioRoot);
            _cityAmbPlayer = new GameObject("CityAmbientPlayer").AddComponent<CriAtomSource>();
            _cityAmbPlayer.transform.SetParent(_audioRoot);
            LoadAudio();
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadDataEvent>(InitializeAudio);
            EventCenter.RemoveListener<LoadSceneFinishedEvent>(PlayAmbientSound);

            base.OnDestroyed();
        }

        private void LoadAudio()
        {
            CriAtom.AddCueSheet("BGM_Title", $"{Path}/BGM.acb", $"{Path}/BGM.awb");
            CriAtom.AddCueSheet("BGM_GamePlay", $"{Path}/BGM.acb", $"{Path}/BGM.awb");
            CriAtom.AddCueSheet("Ambient", "ambient.acb", "ambient.awb");

            //CriAtom.AddCueSheet("SoundFX", $"{Path}/SoundFX.acb", $"{Path}/SoundFX.awb");
        }

        public void PlayBGM(BGMType type)
        {
            _bgmPlayer.cueSheet = $"BGM_{type}";
            _bgmPlayer.cueName = $"BGM_{type}";

            _bgmPlayer.volume = _bgmVolume;
            _bgmPlayer.Play();
        }
        
        public void PlayBGM(BGMType type, float volume)
        {
            _bgmPlayer.cueSheet = $"BGM_{type}";
            _bgmPlayer.cueName = $"BGM_{type}";

            _bgmPlayer.volume = volume;
            _bgmPlayer.Play();
        }

        public void PlaySFX(SFXType type)
        {
            var source = _audioPool.GetAudioSource(type.ToString());
            source.gameObject.SetActive(true);
            source.volume = _soundVolume;
            source.Play();
        }
        
        public void AdjustBGMVolume(float volume)
        {
            _bgmVolume = volume;
            _bgmPlayer.volume = _bgmVolume;
        }

        public void AdjustSoundVolume(float volume)
        {
            _soundVolume = volume;
        }
        
        public void AdjustAmbientVolume(float volume)
        {
            _ambientPlayer.volume = volume;
        }

        public void AdjustCityAmbVolume(float volume)
        {
            _cityAmbPlayer.volume = volume;
        }

        private void InitializeAudio(LoadDataEvent evt)
        {
            SettingData settingData = new();
            Managers.Get<ISaveDataManager>().GetSettingData(ref settingData);
            _bgmVolume = settingData.bgmVolume;
            _soundVolume = settingData.soundVolume;
        }

        private void PlayAmbientSound(LoadSceneFinishedEvent evt)
        {
            _ambientPlayer.volume = 0f;
            _ambientPlayer.cueSheet = "Ambient";
            _ambientPlayer.cueName = "ambient";
            _ambientPlayer.Play();
            
            _cityAmbPlayer.volume = 0f;
            _cityAmbPlayer.cueSheet = "Ambient";
            _cityAmbPlayer.cueName = "CityAmbient";
            _cityAmbPlayer.Play();
        }
    }
}