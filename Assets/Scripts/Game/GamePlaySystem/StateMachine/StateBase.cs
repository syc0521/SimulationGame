using System;

namespace Game.GamePlaySystem.StateMachine
{
    public abstract class StateBase : IState
    {
        public virtual void OnEnter(params object[] list)
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnLeave(params object[] list)
        {
            
        }
    }
}