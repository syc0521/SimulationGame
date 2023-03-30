using Game.Core;

namespace Game.Audio
{
    public interface IAudioManager : IManager
    {
        void PlayBGM(BGMType type);
        void AdjustBGMVolume(float volume);
        void AdjustSoundVolume(float volume);
    }
}