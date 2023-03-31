using Game.Core;
using UnityEngine;

namespace Game.Audio
{
    public interface IAudioManager : IManager
    {
        Transform AudioRoot { get; }
        void PlayBGM(BGMType type);
        void AdjustBGMVolume(float volume);
        void AdjustSoundVolume(float volume);
    }
}