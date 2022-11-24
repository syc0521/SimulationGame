namespace Game.UI
{
    public interface IUILifePhase
    {
        void OnCreated();
        void OnShown();
        void OnHidden();
        void OnUpdate();
        void OnDestroyed();
    }
}