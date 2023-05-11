using Game.Audio;
using Game.Core;
using Game.Data.Event;
using Game.GamePlaySystem;
using Game.GamePlaySystem.Build;
using Game.UI.Decorator;

namespace Game.UI.Panel.Building
{
    public class RoadBuildingPanel : UIPanel
    {
        public RoadBuildingPanel_Nodes nodes;

        public override void OnCreated()
        {
            base.OnCreated();
            nodes.back_btn.onClick.AddListener(BackToSelect);
            nodes.construct_btn.onClick.AddListener(ConstructRoad);
            nodes.undo_btn.onClick.AddListener(UndoRoad);
            nodes.redo_btn.onClick.AddListener(RedoRoad);
            EventCenter.AddListener<RoadConstructEvent>(CheckButtonState);
        }

        public override void OnShown()
        {
            base.OnShown();
            PlayAnimation();
            BuildingManager.Instance.ConstructRoad();
            CheckButtonState(default);
        }

        public override void OnDestroyed()
        {
            nodes.back_btn.onClick.RemoveListener(BackToSelect);
            nodes.construct_btn.onClick.RemoveListener(ConstructRoad);
            nodes.undo_btn.onClick.RemoveListener(UndoRoad);
            nodes.redo_btn.onClick.RemoveListener(RedoRoad);
            EventCenter.RemoveListener<RoadConstructEvent>(CheckButtonState);
            base.OnDestroyed();
        }

        private void BackToSelect()
        {
            if (BuildingManager.Instance.CanUndoRoad())
            {
                AlertDecorator.OpenAlertPanel("是否放弃建造道路？", true, () =>
                {
                    BuildingManager.Instance.DeleteTempBuilding();
                    CloseSelf();
                });
            }
            else
            {
                BuildingManager.Instance.DeleteTempBuilding();
                CloseSelf();
            }
        }
        
        private void ConstructRoad()
        {
            BuildingManager.Instance.ConstructBuilding();
            Managers.Get<IAudioManager>().PlaySFX(SFXType.Place);
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

        private void CheckButtonState(RoadConstructEvent evt)
        {
            nodes.undo_btn.interactable = BuildingManager.Instance.CanUndoRoad();
            nodes.redo_btn.interactable = BuildingManager.Instance.CanRedoRoad();
            nodes.construct_btn.gameObject.SetActive(BuildingManager.Instance.CanUndoRoad());
        }
        
    }
}