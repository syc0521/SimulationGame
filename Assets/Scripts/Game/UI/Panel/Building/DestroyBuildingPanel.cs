using Game.GamePlaySystem;

namespace Game.UI.Panel.Building
{
    public class DestroyBuildingPanel : UIPanel
    {
        public DestroyBuildingPanel_Nodes nodes;

        public override void OnCreated()
        {
            base.OnCreated();
            BuildingManager.Instance.RemoveBuilding();
            nodes.quit_btn.onClick.AddListener(OnQuitButtonClicked);
        }

        public override void OnDestroyed()
        {
            nodes.quit_btn.onClick.RemoveListener(OnQuitButtonClicked);
            base.OnDestroyed();
        }

        private void OnQuitButtonClicked()
        {
            BuildingManager.Instance.TransitToNormalState();
            CloseSelf();
        }
    }
}