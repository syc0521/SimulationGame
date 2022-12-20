using System;
using System.Collections.Generic;
using System.Linq;
using Game.UI.Module;

namespace Game.UI
{
    public class ModuleCollector
    {
        private List<BaseModule> _modules = new();

        private void RegisterModule()
        {
            _modules.Add(new MainModule());
            _modules.Add(new BuildModule());

            /*var typeGamePlayModule = typeof(BaseModule);
            var result = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeGamePlayModule.IsAssignableFrom(t));
            foreach( var type in result )
            {
                _modules.Add((BaseModule)Activator.CreateInstance(type));
            }*/
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