using System;
using Game.Core;

namespace Game.GamePlaySystem
{
    public abstract class GamePlaySystemBase<T> : Singleton<T>, ILifePhase where T : GamePlaySystemBase<T>
    {
        protected override void Awake()
        {
            base.Awake();
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        private void OnDestroy()
        {
            OnDestroyed();
        }

        public virtual void OnAwake()
        {
            
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