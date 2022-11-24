using System;
using System.Collections.Generic;

namespace Game.Core
{
    public static class Managers
    {
        private static readonly Dictionary<Type, IManager> _managers = new(); // Manager列表

        public static void ClearManager()
        {
            _managers.Clear();
        }

        /// <summary>
        /// 注册Manager
        /// </summary>
        /// <typeparam name="T0">接口</typeparam>
        /// <typeparam name="T1">具体类</typeparam>
        public static void Register<T0, T1>() where T1 : T0, IManager, new()
        {
            if (!_managers.ContainsKey(typeof(T0)))
            {
                var obj = new T1();
                _managers.Add(typeof(T0), obj);
                obj.OnAwake();
            }
        }

        public static void Unregister<T>() where T : IManager
        {
            if (_managers.TryGetValue(typeof(T), out var obj))
            {
                obj.OnDestroyed();
                _managers.Remove(typeof(T));
            }
        }

        public static void Start<T>() where T : IManager
        {
            if (_managers.TryGetValue(typeof(T), out var obj))
            {
                obj.OnStart();
            }
        }

        public static T Get<T>() where T : IManager
        {
            if (_managers.TryGetValue(typeof(T), out var obj))
            {
                return (T)_managers[typeof(T)];
            }

            return default;
        }

        public static void Update<T>() where T : IManager
        {
            if (_managers.TryGetValue(typeof(T), out var obj))
            {
                obj.OnUpdate();
            }
        }
        
        
    }
}