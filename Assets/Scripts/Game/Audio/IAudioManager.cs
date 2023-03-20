using Game.Core;

namespace Game.Audio
{
    public interface IAudioManager : IManager
    {
        void PlayBGM(BGMType type);
    }
}