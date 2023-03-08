using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Game.UI.Widget
{
    public enum StatusType
    {
        Happiness,
        People,
        Coin,
    }
    
    public class StatusWidget : WidgetBase, IPointerClickHandler
    {
        public TextMeshProUGUI amount_txt;
        private Action<StatusWidget> _handler;
        public StatusType Type { get; private set; }
        
        public void SetText(string text)
        {
            amount_txt.text = text;
        }

        public void SetClickHandler(Action<StatusWidget> handler, StatusType type)
        {
            _handler = handler;
            Type = type;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _handler?.Invoke(this);
        }
    }
}