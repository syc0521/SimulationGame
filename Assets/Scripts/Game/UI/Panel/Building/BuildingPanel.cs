using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.GamePlaySystem;
using Game.GamePlaySystem.Backpack;
using Game.GamePlaySystem.Build;
using Game.UI.Component;
using Game.UI.Decorator;
using Game.UI.UISystem;
using Game.UI.ViewData;
using UnityEngine;

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
        private Animation _animation;

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
            _animation.clip.SampleAnimation(gameObject, 0);
            _animation.Play();
            InitTabBar();
            nodes.tabBar.SetSelectedIndex(0);
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
            foreach (var item in buildingData.Where(item => 
                         (type is BuildingType.All || type == item.Value.buildingType) && BuildingManager.Instance.CheckBuildingUnlocked(item.Key)))
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
            // 道路需要进入道路建造模式
            if (id == 4)
            {
                UIManager.Instance.OpenPanel<RoadBuildingPanel>();
                return;
            }
            
            var data = ConfigTable.Instance.GetBuildingData(id);
            var type = data.Currencytype[0];
            if (BackpackManager.Instance.GetBackpackCount(type) >= data.Currencycount[0])
            {
                BuildingManager.Instance.AddBuilding(id);
                EventCenter.DispatchEvent(new ShowHUDEvent { HUDType = HUDType.Build });
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