using Game.Core;
using UnityEngine;

namespace Game.Audio
{
    public interface IAudioManager : IManager
    {
        Transform AudioRoot { get; }
        float SoundVolume { get; }
        void PlayBGM(BGMType type);
        void PlayBGM(BGMType type, float volume);
        void PlaySFX(SFXType type);
        void AdjustBGMVolume(float volume);
        void AdjustSoundVolume(float volume);
    }
}