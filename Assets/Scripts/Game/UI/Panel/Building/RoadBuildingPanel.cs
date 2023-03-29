using Game.GamePlaySystem;
using Game.GamePlaySystem.Build;

namespace Game.UI.Panel.Building
{
    public class RoadBuildingPanel : UIPanel
    {
        public RoadBuildingPanel_Nodes nodes;

        public override void OnCreated()
        {
            base.OnCreated();
            nodes.close_btn.onClick.AddListener(OnQuitButtonClicked);
            nodes.undo_btn.onClick.AddListener(UndoRoad);
            nodes.redo_btn.onClick.AddListener(RedoRoad);
        }

        public override void OnShown()
        {
            base.OnShown();
            BuildingManager.Instance.ConstructRoad();
        }

        public override void OnDestroyed()
        {
            nodes.close_btn.onClick.RemoveListener(OnQuitButtonClicked);
            nodes.undo_btn.onClick.RemoveListener(UndoRoad);
            nodes.redo_btn.onClick.RemoveListener(RedoRoad);
            base.OnDestroyed();
        }
        
        private void OnQuitButtonClicked()
        {
            BuildingManager.Instance.ConstructBuilding();
            CloseSelf();
        }

        private void UndoRoad()
        {
            BuildingManager.Instance.UndoRoad();
        }

        private void RedoRoad()
        {
            BuildingManager.Instance.RedoRoad();
        }
    }
}