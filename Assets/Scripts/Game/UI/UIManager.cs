using System;
using System.Collections.Generic;
using Game.Core;
using Game.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class BasePanelOption
    {
        
    }

    public enum UILayer
    {
        Normal, Top, Scene
    }
    
    public class UIManager : ManagerBase, IUIManager
    {
        public static IUIManager Instance => Managers.Get<IUIManager>();
        private ModuleCollector _modules;
        private UISystemCollector _systems;
        private Dictionary<Type, GameObject> panels = new();

        public override void OnAwake()
        {
            base.OnAwake();
            _systems = new();
            _modules = new();
            _systems.Init();
            _modules.Init();
        }

        public override void OnStart()
        {
            base.OnStart();
            _systems.Start();
            _modules.Start();
        }

        public override void OnDestroyed()
        {
            _modules.Destroy();
            _modules = null;
            
            _systems.Destroy();
            _systems = null;
            
            panels.Clear();
        }

        public T OpenPanel<T>(BasePanelOption option = null) where T : UIPanel
        {
            if (HasPanel<T>())
            {
                panels[typeof(T)].SetActive(true);
                return panels[typeof(T)].GetComponent<UIPanel>() as T;
            }

            return CreatePanel<T>();
        }

        public T CreatePanel<T>(BasePanelOption option = null) where T : UIPanel
        {
            Enum.TryParse(typeof(T).Name, out PanelEnum panelEnum);
            var panel = ConfigTable.Instance.GetPanel(panelEnum);
            var obj = Object.Instantiate(panel, ConfigTable.Instance.GetUIRoot(), true);
            var comp = obj.GetComponent<UIPanel>();
            comp.opt = option;
            panels[typeof(T)] = obj;
            comp.OnCreated();
            return comp as T;
        }

        public bool HasPanel<T>() where T : UIPanel
        {
            return panels.ContainsKey(typeof(T));
        }

        public void DestroyPanel(UIPanel panel)
        {
            if (panels.ContainsKey(panel.GetType()))
            {
                panel.OnDestroyed();
                Object.Destroy(panel.gameObject);
                panels.Remove(panel.GetType());
            }
        }

    }
}