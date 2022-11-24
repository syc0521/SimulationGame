using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core
{
    public static class EventCenter
    {
        private static readonly Dictionary<Type, List<Delegate>> handlers = new();

        public static void AddListener<T>(in Action<T> action) where T : struct, IEvent
        {
            var type = typeof(T);
            if (!handlers.ContainsKey(type))
            {
                handlers[type] = new List<Delegate>(1);
            }
            handlers[type].Add(action);
        }
    
        public static void RemoveListener<T>(in Action<T> action) where T : struct, IEvent
        {
            var type = typeof(T);
            if (handlers.ContainsKey(type))
            {
                handlers[type].Remove(action);
            }
        }

        public static void RemoveAllListener<T>() where T : struct, IEvent
        {
            var type = typeof(T);
            if (!handlers.ContainsKey(type)) return;
            handlers[type].Clear();
            handlers.Remove(type);
        }
    
        public static void DispatchEvent<T>(T @event) where T : struct, IEvent
        {
            var type = typeof(T);
            if (!handlers.ContainsKey(type)) return;
            foreach (var item in handlers[type].ToList())
            {
                (item as Action<T>)?.Invoke(@event);
            }
        }
    }
}

