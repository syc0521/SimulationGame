using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Input;
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
        private Dictionary<Type, GameObject> panelObjects = new();
        private Dictionary<UILayerType, Stack<UIPanel>> panels = new();

        public override void OnAwake()
        {
            base.OnAwake();
            _systems = new();
            _modules = new();
            _systems.Init();
            _modules.Init();

            foreach (var type in Enum.GetValues(typeof(UILayerType)).Cast<UILayerType>())
            {
                panels[type] = new();
            }
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

            foreach (var panel in panelObjects.Values.ToList().Where(panel => panel != null))
            {
                panel.GetComponent<UIPanel>()?.OnDestroyed();
            }
            panelObjects.Clear();
            
            foreach (var type in Enum.GetValues(typeof(UILayerType)).Cast<UILayerType>())
            {
                panels[type].Clear();
                panels[type] = null;
            }
            panels.Clear();
            panels = null;
        }

        public override void OnUpdate()
        {
            _modules.Update();
        }

        public T OpenPanel<T>(BasePanelOption option = null) where T : UIPanel
        {
            var data = ConfigTable.Instance.GetUIPanelData(typeof(T).Name);
            Managers.Get<IInputManager>().SetGestureState((UILayerType)data.Layer is UILayerType.Scene);

            if (HasPanel<T>())
            {
                panelObjects[typeof(T)].SetActive(true);
                var panel = panelObjects[typeof(T)].GetComponent<UIPanel>();
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
                GameObject objResult = handle.Result;
                var data = ConfigTable.Instance.GetUIPanelData(typeof(T).Name);
                var obj = Object.Instantiate(objResult, ConfigTable.Instance.GetUIRoot(data.Layer), false);
                comp = obj.GetComponent<UIPanel>();
                comp.opt = option;
                panelObjects[typeof(T)] = obj;

                if (panels[(UILayerType)data.Layer].TryPeek(out var panel)) // 该层级被覆盖的面板要被隐藏
                {
                    panel.gameObject.SetActive(false);
                }
                panels[(UILayerType)data.Layer].Push(comp);
                
                comp.OnCreated();
                comp.OnShown();
            });
        }

        public bool HasPanel<T>() where T : UIPanel
        {
            return panelObjects.ContainsKey(typeof(T));
        }

        public void DestroyPanel(UIPanel panel)
        {
            if (panelObjects.ContainsKey(panel.GetType()))
            {
                var data = ConfigTable.Instance.GetUIPanelData(panel.GetType().Name);
                panels[(UILayerType)data.Layer].Pop();
                if (panels[(UILayerType)data.Layer].TryPeek(out var lastPanel)) // 该层级被覆盖的面板要显示
                {
                    lastPanel.gameObject.SetActive(true);
                }
                panel.OnDestroyed();
                Object.Destroy(panel.gameObject);
                panelObjects.Remove(panel.GetType());
                Managers.Get<IInputManager>().SetGestureState(GetTopLayer() is UILayerType.Scene);
            }
        }

        private UILayerType GetTopLayer()
        {
            return panels[UILayerType.Top].Count > 0 ? UILayerType.Top :
                panels[UILayerType.Full].Count > 0 ? UILayerType.Full :
                panels[UILayerType.Pop].Count > 0 ? UILayerType.Pop : UILayerType.Scene;
        }

    }
}