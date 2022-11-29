namespace Game.UI
{
    public abstract class BaseModule
    {
        public abstract void OnCreated();

        public virtual void OnStart() { }

        public abstract void OnDestroyed();
    }
}