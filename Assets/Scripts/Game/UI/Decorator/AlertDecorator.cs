using System;
using Game.UI.Panel.Common;

namespace Game.UI.Decorator
{
    public static class AlertDecorator
    {
        public static void OpenAlertPanel(string content, bool hasCancel, Action handler = null)
        {
            UIManager.Instance.OpenPanel<AlertPanel>(new AlertPanelOption
            {
                content = content,
                clickHandler = handler,
                hasCancel = hasCancel,
            });
        }
    }
}