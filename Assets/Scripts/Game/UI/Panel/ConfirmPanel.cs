using System;
using Game.Core;
using Game.Data.Event;
using Game.GamePlaySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Panel
{
    public class ConfirmPanel : UIPanel
    {
        public Button confirm, cancel, rotate;

        public override void OnCreated()
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
            UIManager.Instance.DestroyPanel(this);
        }

        private void OnCancelClicked()
        {
            BuildingManager.Instance.DeleteTempBuilding();
            UIManager.Instance.DestroyPanel(this);
        }

        private void OnRotateClicked()
        {
            BuildingManager.Instance.RotateBuilding();
        }
    }
}