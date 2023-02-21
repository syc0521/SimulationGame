using System;
using UnityEngine.UI;

namespace Game.UI.Widget
{
    public class CustomSliderWidget : WidgetBase
    {
        public Slider slider;
        private Action<float> _handler;

        public override void OnCreated()
        {
            base.OnCreated();
            slider.onValueChanged.AddListener(SliderValueChanged);
        }

        public override void OnDestroyed()
        {
            slider.onValueChanged.RemoveListener(SliderValueChanged);
            base.OnDestroyed();
        }

        private void SliderValueChanged(float f)
        {
            _handler?.Invoke(f);
        }

        public void SetHandler(Action<float> handler)
        {
            _handler = handler;
        }

        public float GetValue => slider.value;
    }
}