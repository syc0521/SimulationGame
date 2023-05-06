using Game.Data.Event;

namespace Game.UI.Panel
{
    public partial class MainPanel
    {
        private void ChangeHUDStatus(ShowHUDEvent evt)
        {
            switch (evt.HUDType)
            {
                case HUDType.All:
                    break;
                case HUDType.Build:
                    break;
            }
        }
    }
}