using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.Data.FeatureOpen;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.FeatureOpen;
using Game.UI.Decorator;
using Game.UI.Panel;
using Game.UI.Panel.Building;
using Game.UI.Panel.Shop;
using UnityEngine;

namespace Game.UI.Module
{
    public class BuildModule : BaseModule
    {
        public override void OnCreated()
        {
            EventCenter.AddListener<OpenBuildingInfoEvent>(OpenBuildingInfo);
            EventCenter.AddListener<DestroyEvent>(ShowDestroyAlert);
        }
        
        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<OpenBuildingInfoEvent>(OpenBuildingInfo);
            EventCenter.RemoveListener<DestroyEvent>(ShowDestroyAlert);
        }

        private void ShowDestroyAlert(DestroyEvent evt)
        {
            AlertDecorator.OpenAlertPanel("是否删除该建筑", true, evt.handler);
        }

        private void OpenBuildingInfo(OpenBuildingInfoEvent evt)
        {
            var data = BuildingManager.Instance.GetBuildingData((uint)evt.id);
            var buildingData = ConfigTable.Instance.GetBuildingData(data.type);
            if (buildingData == null || buildingData.Buildingid == 4) // 道路
            {
                return;
            }

            if (buildingData.Buildingid == 13 && FeatureOpenManager.Instance.HasFeature(FeatureType.Mall)) // 商城
            {
                UIManager.Instance.OpenPanel<MallPanel>();
                return;
            }
            
            if (evt is { isStatic: true, id: 10001 } && FeatureOpenManager.Instance.HasFeature(FeatureType.Government)) // 官府
            {
                UIManager.Instance.OpenPanel<GovernmentPanel>();
                return;
            }
            
            EventCenter.DispatchEvent(new OpenBuildingDetailEvent
            {
                id = evt.id
            });
        }

    }
}