using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.Data.Event;
using Game.Data.Event.FeatureOpen;
using Game.Data.FeatureOpen;

namespace Game.GamePlaySystem.FeatureOpen
{
    public class FeatureOpenManager : GamePlaySystemBase<FeatureOpenManager>
    {
        private HashSet<FeatureType> unlockedFeatures = new();
        public override void OnAwake()
        {
            base.OnAwake();
            EventCenter.AddListener<LoadDataEvent>(InitData);
        }

        public override void OnDestroyed()
        {
            EventCenter.RemoveListener<LoadDataEvent>(InitData);
            base.OnDestroyed();
        }

        private void InitData(LoadDataEvent evt)
        {
            Managers.Get<ISaveDataManager>().GetUnlockedFeatures(ref unlockedFeatures);
        }

        public void OpenFeature(FeatureType type)
        {
            if (HasFeature(type))
            {
                return;
            }

            unlockedFeatures.Add(type);
            EventCenter.DispatchEvent(new UnlockFeatureEvent
            {
                type = type,
            });
        }

        public bool HasFeature(FeatureType type)
        {
            return unlockedFeatures.Contains(type);
        }
        
    }
}