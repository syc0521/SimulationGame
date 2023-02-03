using Game.Core;

namespace Game.Input
{
    public interface IInputManager : IManager
    {
        bool IsPointerOverGameObject();
        void SetGestureState(bool enabled);
    }
}