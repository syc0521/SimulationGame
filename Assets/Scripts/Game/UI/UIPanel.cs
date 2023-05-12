using System;
using Game.Audio;
using Game.Core;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.UI
{
    public abstract class UIPanel : MonoBehaviour, IUILifePhase
    {
        public BasePanelOption opt;
        protected Animation _animation;

        private void Update()
        {
            OnUpdate();
        }

        public virtual void OnCreated()
        {
            TryGetComponent(out Animation anim);
            _animation = anim;
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

        protected void PlayAnimation()
        {
            if (_animation != null)
            {
                _animation.clip.SampleAnimation(gameObject, 0);
                _animation.Play();
            }
        }
    }
}