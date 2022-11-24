namespace Game.Core
{
    public interface ILifePhase
    {
        void OnAwake();
        void OnStart();
        void OnUpdate();
        void OnDestroyed();
    }
}