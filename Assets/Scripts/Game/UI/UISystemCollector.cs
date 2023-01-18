using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.UI
{
    public class UISystemCollector
    {
        private List<IUISystem> _systems = new();

        private void RegisterSystem()
        {
            var typeGamePlayModule = typeof(IUISystem);
            var result = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeGamePlayModule.IsAssignableFrom(t));
            foreach( var type in result )
            {
                if (type == typeGamePlayModule || type.IsAbstract)
                {
                    continue;
                }
                var constructorInfo = type.GetConstructor(new Type[] {});
                _systems.Add((IUISystem)constructorInfo?.Invoke(new object[] {}));
            }
            
        }

        public void Init()
        {
            RegisterSystem();
            foreach (var system in _systems)
            {
                system.OnAwake();
            }
        }
        
        public void Start()
        {
            foreach (var system in _systems)
            {
                system.OnStart();
            }
        }

        public void Destroy()
        {
            foreach (var system in _systems.ToList())
            {
                system.OnDestroyed();
                _systems.Remove(system);
            }

            _systems = null;
        }
    }
}