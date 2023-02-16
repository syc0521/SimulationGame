using System;
using TMPro;
using UnityEngine.UI;

namespace Game.UI.Widget
{
    public class TabWidget : WidgetBase
    {
        public Toggle button;
        public TextMeshProUGUI text;
        private Action<int> _clickHandler;
        private int id;

        public override void OnShown()
        {
            base.OnShown();
            button.onValueChanged.AddListener(OnClick);
        }

        public override void OnDestroyed()
        {
            button.onValueChanged.RemoveListener(OnClick);
            base.OnDestroyed();
        }

        public void SetText(string txt)
        {
            text.text = txt;
        }

        public void SetClickHandler(int index, Action<int> handler)
        {
            id = index;
            _clickHandler = handler;
        }

        private void OnClick(bool b)
        {
            if (b)
            {
                _clickHandler?.Invoke(id);
            }
        }

        public void SetToggleGroup(ToggleGroup group)
        {
            button.group = group;
        }

        public void SetOn()
        {
            button.isOn = true;
            OnClick(true);
        }
    }
}