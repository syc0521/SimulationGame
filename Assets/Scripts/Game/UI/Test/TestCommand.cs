using System;
using Game.Core;
using Game.Data.Event;
using Game.GamePlaySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TestCommand : MonoBehaviour
    {
        public Button build1, build2, select, destroy1, destroy2;

        private void Start()
        {
            EventCenter.AddListener<SelectEvent>(OnBuildingSelected);
            build1.onClick.AddListener(BuildHandler);
            build2.onClick.AddListener(Build2Handler);
            destroy1.onClick.AddListener(DestroyHandler);
            select.onClick.AddListener(SelectHandler);
        }

        private void BuildHandler()
        {
            BuildingManager.Instance.AddBuilding(0);
        }
        
        private void Build2Handler()
        {
            BuildingManager.Instance.AddBuilding(1);
        }

        private void DestroyHandler()
        {
            EventCenter.DispatchEvent(new BuildEvent{ buildState = BuildState.Delete });
        }

        private void SelectHandler()
        {
            BuildingManager.Instance.SelectBuilding();
        }

        private void OnBuildingSelected(SelectEvent evt)
        {
            Debug.Log(evt.position);
        }
        
    }
}
