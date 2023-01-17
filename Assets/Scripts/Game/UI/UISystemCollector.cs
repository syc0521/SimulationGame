using System.Collections.Generic;
using System.Linq;

namespace Game.UI
{
    public class UISystemCollector
    {
        private List<UISystemBase> _systems = new();

        private void RegisterSystem()
        {
            
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