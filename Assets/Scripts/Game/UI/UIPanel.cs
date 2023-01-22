using System;
using Game.Core;
using UnityEngine;

namespace Game.UI
{
    public abstract class UIPanel : MonoBehaviour, IUILifePhase
    {
        public BasePanelOption opt;

        private void Update()
        {
            OnUpdate();
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