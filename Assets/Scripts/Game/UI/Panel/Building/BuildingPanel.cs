using System;
using Game.Data;
using Game.GamePlaySystem;
using Game.UI.Component;
using Game.UI.UISystem;
using Game.UI.ViewData;

namespace Game.UI.Panel.Building
{
    public class BuildingListData : ListData
    {
        public int id;
        public BuildingViewData data;
        public Action<int> clickHandler;
    }
    
    public class BuildingPanel : UIPanel
    {
        public BuildingPanel_Nodes nodes;

        public override void OnCreated()
        {
            nodes.close_btn.onClick.AddListener(ClosePanel);
        }

        public override void OnShown()
        {
            base.OnShown();
            InitBuilding();
        }

        public override void OnDestroyed()
        {
            nodes.close_btn.onClick.RemoveListener(ClosePanel);
            base.OnDestroyed();
        }
        
        private void ClosePanel()
        {
            CloseSelf();
        }

        private void InitBuilding()
        {
            nodes.building_list.Clear();
            var buildingData = BuildingSystem.Instance.GetAllBuildingViewData();
            foreach (var item in buildingData)
            {
                nodes.building_list.AddItem(new BuildingListData
                {
                    id = item.Key,
                    data = item.Value,
                    clickHandler = ClickBuilding
                });
            }
        }

        private void ClickBuilding(int id)
        {
            BuildingManager.Instance.AddBuilding(id);
            CloseSelf();
        }
    }
}