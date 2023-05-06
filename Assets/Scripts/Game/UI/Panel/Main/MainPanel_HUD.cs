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
                    nodes.operator_go.SetActive(true);
                    nodes.task_go.SetActive(true);
                    nodes.status_go.SetActive(true);
                    nodes.pause_go.SetActive(true);
                    break;
                case HUDType.Build:
                    nodes.operator_go.SetActive(false);
                    nodes.task_go.SetActive(false);
                    nodes.status_go.SetActive(false);
                    break;
            }
        }
    }
}