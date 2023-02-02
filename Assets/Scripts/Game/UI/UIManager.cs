using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.LevelAndEntity.ResLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class BasePanelOption
    {
        
    }

    public enum UILayerType
    {
        Scene = 0,
        Pop = 1,
        Full = 2,
        Top = 3,
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

            foreach (var panel in panels.Values.ToList().Where(panel => panel != null))
            {
                panel.GetComponent<UIPanel>()?.OnDestroyed();
            }
            panels.Clear();
        }

        public override void OnUpdate()
        {
            _modules.Update();
        }

        public T OpenPanel<T>(BasePanelOption option = null) where T : UIPanel
        {
            if (HasPanel<T>())
            {
                panels[typeof(T)].SetActive(true);
                var panel = panels[typeof(T)].GetComponent<UIPanel>();
                panel.OnShown();
                return panel as T;
            }

            CreatePanel<T>(option);
            return default;
        }

        private void CreatePanel<T>(BasePanelOption option = null) where T : UIPanel
        {
            UIPanel comp;
            Managers.Get<IResLoader>().LoadRes(ResEnum.Panel, typeof(T).Name, handle =>
            {
                GameObject panel = handle.Result;
                var data = ConfigTable.Instance.GetUIPanelData(typeof(T).Name);
                var obj = Object.Instantiate(panel, ConfigTable.Instance.GetUIRoot(data.Layer), false);
                comp = obj.GetComponent<UIPanel>();
                comp.opt = option;
                panels[typeof(T)] = obj;
                comp.OnCreated();
                comp.OnShown();
            });
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