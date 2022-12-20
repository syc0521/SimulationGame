namespace Game.UI
{
    public abstract class BaseModule
    {
        public BaseModule(){}
        public abstract void OnCreated();

        public virtual void OnStart() { }

        public abstract void OnDestroyed();
    }
}