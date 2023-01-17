using System;
using Game.Core;
using UnityEngine;

namespace Game.UI
{
    public abstract class UIPanel : MonoBehaviour, IUILifePhase
    {
        public BasePanelOption opt;

        private void Awake()
        {
            OnCreated();
        }

        private void Start()
        {
            OnShown();
        }

        private void Update()
        {
            OnUpdate();
        }

        private void OnDestroy()
        {
            OnDestroyed();
        }

        public virtual void OnCreated()
        {
            
        }

        public virtual void OnShown()
        {
            
        }

        public virtual void OnHidden()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnDestroyed()
        {
            
        }

        protected void CloseSelf()
        {
            UIManager.Instance.DestroyPanel(this);
        }
    }
}