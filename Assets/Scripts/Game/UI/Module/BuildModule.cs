﻿using Game.Audio;
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
using Game.UI.Panel.Task;
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

            Managers.Get<IAudioManager>().PlaySFX(SFXType.Button2);
            if (buildingData.Buildingid == 13 && FeatureOpenManager.Instance.HasFeature(FeatureType.Mall)) // 商城
            {
                UIManager.Instance.OpenPanel<MallPanel>();
                return;
            }
            
            if (buildingData.Buildingid == 28 && FeatureOpenManager.Instance.HasFeature(FeatureType.ItemSell)) // 小摊
            {
                UIManager.Instance.OpenPanel<ItemSellPanel>();
                return;
            }
            
            if (buildingData.Buildingid == 29 && FeatureOpenManager.Instance.HasFeature(FeatureType.DailyTask)) // 任务
            {
                UIManager.Instance.OpenPanel<DailyTaskPanel>();
                return;
            }
            
            if (evt is { isStatic: true, id: 10001 }) // 官府
            {
                if (FeatureOpenManager.Instance.HasFeature(FeatureType.Government))
                {
                    UIManager.Instance.OpenPanel<GovernmentPanel>();
                }
                return;
            }
            
            EventCenter.DispatchEvent(new OpenBuildingDetailEvent
            {
                id = evt.id
            });
        }

    }
}