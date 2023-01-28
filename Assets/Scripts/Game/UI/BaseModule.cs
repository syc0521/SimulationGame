namespace Game.UI
{
    public abstract class BaseModule
    {
        public BaseModule(){}
        public abstract void OnCreated();

        public virtual void OnStart() { }
        
        public virtual void OnUpdate() { }

        public abstract void OnDestroyed();
    }
}