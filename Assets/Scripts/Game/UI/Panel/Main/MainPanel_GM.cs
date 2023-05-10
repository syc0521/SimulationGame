using Game.UI.Panel.GM;

namespace Game.UI.Panel
{
    public partial class MainPanel
    {
        private int _triggerCount = 0;
        private const int maxCount = 8;
        
        private void TriggerGMPanel()
        {
            _triggerCount++;
            if (_triggerCount >= maxCount)
            {
                _triggerCount = 0;
                UIManager.Instance.OpenPanel<GMPanel>();
            }
        }
    }
}