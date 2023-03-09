using System;
using Game.Core;
using Game.Data.Event;
using Game.Data.FeatureOpen;
using Game.GamePlaySystem.FeatureOpen;
using Game.GamePlaySystem.StateMachine;
using Game.Input;
using Game.LevelAndEntity.Component;
using Unity.Entities;
using UnityEngine;

namespace Game.GamePlaySystem.GameState
{
    public class NormalState : StateBase
    {
        public override void OnEnter(params object[] list)
        {
            EventCenter.AddListener<TouchEvent>(OpenBuildingInfo);
            EventCenter.AddListener<LongPressEvent>(ModifyBuilding);
        }

        public override void OnLeave(params object[] list)
        {
            EventCenter.RemoveListener<TouchEvent>(OpenBuildingInfo);
            EventCenter.RemoveListener<LongPressEvent>(ModifyBuilding);
        }
        
        private void ModifyBuilding(LongPressEvent evt)
        {
            if (BuildingManager.Instance.DetectDynamicBuilding(evt.pos, out var entity) && FeatureOpenManager.Instance.HasFeature(FeatureType.Move))
            {
                BuildingManager.Instance.ModifyBuilding(entity);
            }
        }

        private void OpenBuildingInfo(TouchEvent evt)
        {
            if (BuildingManager.Instance.DetectBuilding(evt.pos, out var entity))
            {
                var levelObject = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LevelObject>(entity);
                EventCenter.DispatchEvent(new OpenBuildingInfoEvent
                {
                    id = (int)levelObject.id,
                    isStatic = levelObject.isStatic,
                });
            }
        }
    }
}