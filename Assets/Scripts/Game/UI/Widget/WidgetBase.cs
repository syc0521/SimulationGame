using UnityEngine;

namespace Game.UI.Widget
{
    public abstract class WidgetBase : MonoBehaviour
    {
        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnStart() { }

        protected virtual void OnUpdate() { }

    }
}