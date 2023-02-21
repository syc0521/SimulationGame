using System;
using UnityEngine;

namespace Game.UI.Widget
{
    public abstract class WidgetBase : MonoBehaviour, IUILifePhase
    {
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

        public virtual void OnCreated() { }

        public virtual void OnShown() { }

        /// <summary>
        /// widget没用
        /// </summary>
        public void OnHidden() { }

        public virtual void OnUpdate() { }
        
        public virtual void OnDestroyed() { }

    }
}