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
            _modules.Add(new BuildModule());

        }

        public void Init()
        {
            RegisterModule();
            foreach (var module in _modules)
            {
                module.OnCreated();
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