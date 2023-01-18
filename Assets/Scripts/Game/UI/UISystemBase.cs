using Game.Core;

namespace Game.UI
{
    public abstract class UISystemBase<T> : IUISystem where T : UISystemBase<T>
    {
        public static T Instance { get; private set; }
        public virtual void OnAwake()
        {
            Instance = this as T;
        }

        public virtual void OnStart()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnDestroyed()
        {
            
        }
    }
}