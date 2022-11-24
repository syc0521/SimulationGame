namespace Game.GamePlaySystem.StateMachine
{
    public interface IState
    {
        void OnEnter(params object[] list);
        void OnUpdate();
        void OnLeave(params object[] list);
    }
}