using System;
using System.Collections.Generic;

namespace Game.GamePlaySystem.StateMachine
{
    public class StateMachine : IDisposable
    {
        private Dictionary<Type, IState> stateDic;
        private IState currentState;

        public StateMachine()
        {
            stateDic = new();
        }
        
        public StateMachine(IReadOnlyList<IState> states) : this()
        {
            foreach (var state in states)
            {
                var type = state.GetType();
                Register(state, type);
            }
        }

        public void Register<T>(T state) where T : IState
        {
            if (!stateDic.ContainsKey(typeof(T)))
            {
                stateDic[typeof(T)] = state;
            }
        }
        
        private void Register(IState state, Type type)
        {
            if (!stateDic.ContainsKey(type))
            {
                stateDic[type] = state;
            }
        }

        public void ChangeState<T>(params object[] list) where T : IState
        {
            if (stateDic.ContainsKey(typeof(T)))
            {
                currentState?.OnLeave(list);
                stateDic[typeof(T)]?.OnEnter(list);
                currentState = stateDic[typeof(T)];
            }
        }

        public void UnRegister<T>() where T : IState
        {
            if (stateDic.ContainsKey(typeof(T)))
            {
                if (currentState == stateDic[typeof(T)])
                {
                    currentState?.OnLeave();
                }

                stateDic.Remove(typeof(T));
            }
        }

        public string GetCurrentState() => currentState.GetType().Name;

        public void Dispose()
        {
            currentState?.OnLeave();
            currentState = null;
        }
    }
}