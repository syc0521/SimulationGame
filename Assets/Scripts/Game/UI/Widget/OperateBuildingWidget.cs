using Game.Data.FeatureOpen;
using Game.GamePlaySystem;
using Game.GamePlaySystem.FeatureOpen;
using UnityEngine.UI;

namespace Game.UI.Widget
{
    public class OperateBuildingWidget : WidgetBase
    {
        public Button confirm, cancel, rotate;
        public override void OnShown()
        {
            confirm.onClick.AddListener(OnConfirmClicked);
            cancel.onClick.AddListener(OnCancelClicked);
            rotate.onClick.AddListener(OnRotateClicked);
        }
        
        public override void OnDestroyed()
        {
            confirm.onClick.RemoveListener(OnConfirmClicked);
            cancel.onClick.RemoveListener(OnCancelClicked);
            rotate.onClick.RemoveListener(OnRotateClicked);
        }

        public override void OnUpdate()
        {
            transform.position = BuildingManager.Instance.ScreenPos;
        }

        private void OnConfirmClicked()
        {
            BuildingManager.Instance.ConstructBuilding();
            gameObject.SetActive(false);
        }

        private void OnCancelClicked()
        {
            BuildingManager.Instance.DeleteTempBuilding();
            gameObject.SetActive(false);
        }

        private void OnRotateClicked()
        {
            BuildingManager.Instance.RotateBuilding();
        }

        public void ShowConfirmButton(bool value)
        {
            confirm.gameObject.SetActive(value);
        }

        public void RefreshButtons()
        {
            rotate.gameObject.SetActive(FeatureOpenManager.Instance.HasFeature(FeatureType.Rotate));
        }
    }
}