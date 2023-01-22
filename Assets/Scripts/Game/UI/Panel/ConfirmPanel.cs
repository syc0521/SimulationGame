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
            EventCenter.AddListener<BuildUIEvent>(RefreshUIPos);
            confirm.onClick.AddListener(OnConfirmClicked);
            cancel.onClick.AddListener(OnCancelClicked);
            rotate.onClick.AddListener(OnRotateClicked);
        }
        
        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<BuildUIEvent>(RefreshUIPos);
            confirm.onClick.RemoveListener(OnConfirmClicked);
            cancel.onClick.RemoveListener(OnCancelClicked);
            rotate.onClick.RemoveListener(OnRotateClicked);
        }

        private void RefreshUIPos(BuildUIEvent obj)
        {
            if (transform != null)
            {
                ChangeUIPos(obj.pos);
            }
        }

        public void ChangeUIPos(Vector3 pos)
        {
            transform.position = pos;
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
            EventCenter.DispatchEvent(new RotateEvent());
        }
    }
}