using Game.Core;
using Game.Data.Event;
using Game.UI.Decorator;
using Game.UI.Panel;
using UnityEngine;

namespace Game.UI.Module
{
    public class BuildModule : BaseModule
    {
        public override void OnCreated()
        {
            EventCenter.AddListener<DestroyEvent>(ShowDestroyAlert);
        }
        
        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<DestroyEvent>(ShowDestroyAlert);
        }

        private void ShowDestroyAlert(DestroyEvent evt)
        {
            AlertDecorator.OpenAlertPanel("是否删除该建筑", true, evt.handler);
        }

    }
}