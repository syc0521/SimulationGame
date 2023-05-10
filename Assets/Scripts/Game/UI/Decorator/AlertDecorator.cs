using System;
using Game.UI.Panel.Building;
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

        public static void OpenAlertUpgradePanel(int staticId, int curLevel, bool isGov = false)
        {
            UIManager.Instance.OpenPanel<AlertUpgradePanel>(new AlertUpgradePanelOption
            {
                staticId = staticId,
                currentLevel = curLevel,
                isGov = isGov
            });
        }
    }
}