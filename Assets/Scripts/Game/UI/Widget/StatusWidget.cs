using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Game.UI.Widget
{
    public class StatusWidget : WidgetBase, IPointerClickHandler
    {
        public TextMeshProUGUI amount_txt;
        private Action<Transform> _handler;
        
        public void SetText(string text)
        {
            amount_txt.text = text;
        }

        public void SetClickHandler(Action<Transform> handler)
        {
            _handler = handler;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _handler?.Invoke(gameObject.transform);
        }
    }
}