using System;
using Game.UI.Panel.Common;

namespace Game.UI.Decorator
{
    public static class AlertDecorator
    {
        public static void OpenAlertPanel(string content, Action handler)
        {
            UIManager.Instance.OpenPanel<AlertPanel>(new AlertPanelOption
            {
                content = content,
                clickHandler = handler
            });
        }
    }
}