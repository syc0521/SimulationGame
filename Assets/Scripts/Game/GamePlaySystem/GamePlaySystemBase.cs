using System;
using Game.Core;

namespace Game.GamePlaySystem
{
    public abstract class GamePlaySystemBase<T> : IGamePlaySystem where T : GamePlaySystemBase<T>
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