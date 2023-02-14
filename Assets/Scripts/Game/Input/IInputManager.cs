using Game.Core;

namespace Game.Input
{
    public interface IInputManager : IManager
    {
        void SetGestureState(bool enabled);
    }
}