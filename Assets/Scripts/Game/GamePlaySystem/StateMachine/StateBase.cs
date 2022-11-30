using System;

namespace Game.GamePlaySystem.StateMachine
{
    public abstract class StateBase : IState
    {
        public abstract void OnEnter(params object[] list);

        public virtual void OnUpdate()
        {
            
        }

        public abstract void OnLeave(params object[] list);
    }
}