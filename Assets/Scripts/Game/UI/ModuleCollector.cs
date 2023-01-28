using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.UI
{
    public class ModuleCollector
    {
        private List<BaseModule> _modules = new();

        private void RegisterModule()
        {
            var typeGamePlayModule = typeof(BaseModule);
            var result = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeGamePlayModule.IsAssignableFrom(t));
            foreach( var type in result )
            {
                if (type == typeGamePlayModule)
                {
                    continue;
                }
                var constructorInfo = type.GetConstructor(new Type[] {});
                _modules.Add((BaseModule)constructorInfo?.Invoke(new object[] {}));
            }
        }

        public void Init()
        {
            RegisterModule();
            foreach (var module in _modules)
            {
                module.OnCreated();
            }
        }

        public void Start()
        {
            foreach (var module in _modules)
            {
                module.OnStart();
            }
        }

        public void Update()
        {
            foreach (var module in _modules)
            {
                module.OnUpdate();
            }
        }

        public void Destroy()
        {
            foreach (var module in _modules.ToList())
            {
                module.OnDestroyed();
                _modules.Remove(module);
            }

            _modules = null;
        }
    }
}