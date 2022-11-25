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
        public Button confirm, cancel;

        public override void OnCreated()
        {
            EventCenter.AddListener<BuildUIEvent>(RefreshUIPos);
            confirm.onClick.AddListener(OnConfirmClicked);
            cancel.onClick.AddListener(OnCancelClicked);
        }
        
        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<BuildUIEvent>(RefreshUIPos);
        }

        private void RefreshUIPos(BuildUIEvent obj)
        {
            ChangeUIPos(obj.pos);
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
    }
}