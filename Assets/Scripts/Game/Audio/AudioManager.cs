using System;
using CriWare;
using Game.Core;
using Game.Data.Event.Audio;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Audio
{
    public class AudioManager : ManagerBase, IAudioManager
    {
        private CriAtomSource _bgmPlayer;
        private const string Path = "Audio/PC";
        public override void OnAwake()
        {
            base.OnAwake();
            _bgmPlayer = new GameObject("BGMPlayer").AddComponent<CriAtomSource>();
            _bgmPlayer.volume = 0.8f;
            LoadBGM();
        }

        private void LoadBGM()
        {
            CriAtom.AddCueSheet($"BGM_Title", $"{Path}/BGM.acb", $"{Path}/BGM.awb");
            CriAtom.AddCueSheet($"BGM_GamePlay", $"{Path}/BGM.acb", $"{Path}/BGM.awb");
        }

        public void PlayBGM(BGMType type)
        {
            _bgmPlayer.cueSheet = $"BGM_{type}";
            _bgmPlayer.cueName = $"BGM_{type}";
            
            _bgmPlayer.Play();
        }
        
        public void AdjustBGMVolume(float volume)
        {
            _bgmPlayer.volume = volume;
        }

        public void AdjustSoundVolume(float volume)
        {
            
        }
    }
}