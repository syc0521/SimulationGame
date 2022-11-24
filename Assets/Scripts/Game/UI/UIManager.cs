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
        private Dictionary<Type, GameObject> panels = new();

        public override void OnAwake()
        {
            base.OnAwake();
            _modules = new();
            _modules.Init();
        }

        public override void OnDestroyed()
        {
            _modules.Destroy();
            _modules = null;
        }

        public T OpenPanel<T>(BasePanelOption option = null) where T : UIPanel
        {
            
            return default;
        }

        public void CreatePanel<T>(BasePanelOption option = null) where T : UIPanel
        {
            Enum.TryParse(typeof(T).Name, out PanelEnum panelEnum);
            var panel = Config.Instance.GetPanel(panelEnum);
            var obj = Object.Instantiate(panel, Config.Instance.GetUIRoot(), true);
            var comp = obj.GetComponent<UIPanel>();
            comp.opt = option;
            panels[typeof(T)] = obj;
            comp.OnCreated();
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