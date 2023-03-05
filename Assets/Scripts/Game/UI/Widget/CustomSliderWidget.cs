using System;
using Game.UI.Component;
using Game.UI.Panel.Task;
using TMPro;
using UnityEngine.UI;

namespace Game.UI.Widget
{
    public class CustomSliderWidget : WidgetBase, IListWidget
    {
        public Slider slider;
        public TextMeshProUGUI detail, amount;
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

        public void RemoveHandler()
        {
            _handler = null;
        }

        public void SetValue(float f)
        {
            slider.value = f;
        }

        public void SetDetailText(string s)
        {
            detail.text = s;
        }

        public void SetAmountText(string s)
        {
            amount.text = s;
        }

        public float GetValue => slider.value;

        public void Refresh(ListData data)
        {
            if (data is TaskDetailData taskDetailData)
            {
                SetDetailText(taskDetailData.name);
                SetAmountText($"{taskDetailData.current}/{taskDetailData.amount}");
                SetValue(taskDetailData.current / (float)taskDetailData.amount);
            }
        }
    }
}