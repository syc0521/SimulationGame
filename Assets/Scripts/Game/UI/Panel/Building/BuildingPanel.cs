using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data;
using Game.GamePlaySystem;
using Game.GamePlaySystem.Backpack;
using Game.UI.Component;
using Game.UI.Decorator;
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

        private List<string> tabTitle = new()
        {
            "全部", "房屋", "生产", "装饰", "地标"
        };

        public override void OnCreated()
        {
            nodes.close_btn.onClick.AddListener(ClosePanel);
        }

        public override void OnShown()
        {
            base.OnShown();
            InitTabBar();
            InitBuilding(BuildingType.All);
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

        private void InitBuilding(BuildingType type)
        {
            nodes.building_list.Clear();
            var buildingData = BuildingSystem.Instance.GetAllBuildingViewData();
            foreach (var item in buildingData.Where(data => data.Value.isUnlock))
            {
                if (type == 0 || type == item.Value.buildingType)
                {
                    nodes.building_list.AddItem(new BuildingListData
                    {
                        id = item.Key,
                        data = item.Value,
                        clickHandler = ClickBuilding
                    });
                }
            }
        }

        private void ClickBuilding(int id)
        {
            var data = ConfigTable.Instance.GetBuildingData(id);
            var type = data.Currencytype[0];
            if (BackpackManager.Instance.GetBackpackCount(type) >= data.Currencycount[0])
            {
                BuildingManager.Instance.AddBuilding(id);
                CloseSelf();
            }
            else
            {
                AlertDecorator.OpenAlertPanel("建筑材料不足！", false);
            }
            
        }

        private void InitTabBar()
        {
            nodes.tabBar.SetData(tabTitle, ClickTab);
        }

        private void ClickTab(int id)
        {
            InitBuilding((BuildingType)id);
        }
    }
}