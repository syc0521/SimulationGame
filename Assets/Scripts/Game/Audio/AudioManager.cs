using System;
using CriWare;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.Data.Event.Audio;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Game.Audio
{
    public class AudioManager : ManagerBase, IAudioManager
    {
        public Transform AudioRoot => _audioRoot;
        private Transform _audioRoot;
        private CriAtomSource _bgmPlayer;
        private const string Path = "Audio/PC";
        private float _bgmVolume, _soundVolume;
        private ObjectPool<GameObject> _audioPool;
        public override void OnAwake()
        {
            base.OnAwake();
            _audioRoot = GameObject.Find("CRIWARE").transform;
            EventCenter.AddListener<LoadDataEvent>(LoadVolume);
            _bgmPlayer = new GameObject("BGMPlayer").AddComponent<CriAtomSource>();
            _bgmPlayer.transform.SetParent(_audioRoot);
            LoadAudio();
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadDataEvent>(LoadVolume);
            base.OnDestroyed();
        }

        private void LoadAudio()
        {
            CriAtom.AddCueSheet($"BGM_Title", $"{Path}/BGM.acb", $"{Path}/BGM.awb");
            CriAtom.AddCueSheet($"BGM_GamePlay", $"{Path}/BGM.acb", $"{Path}/BGM.awb");
            //CriAtom.AddCueSheet("SoundFX", $"{Path}/SoundFX.acb", $"{Path}/SoundFX.awb");
        }

        public void PlayBGM(BGMType type)
        {
            _bgmPlayer.cueSheet = $"BGM_{type}";
            _bgmPlayer.cueName = $"BGM_{type}";

            _bgmPlayer.volume = _bgmVolume;
            _bgmPlayer.Play();
        }

        public void PlaySFX(SFXType type)
        {
            // todo 维护一个音频对象池
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

        private void LoadVolume(LoadDataEvent evt)
        {
            SettingData settingData = new();
            Managers.Get<ISaveDataManager>().GetSettingData(ref settingData);
            _bgmVolume = settingData.bgmVolume;
            _soundVolume = settingData.soundVolume;
        }
    }
}