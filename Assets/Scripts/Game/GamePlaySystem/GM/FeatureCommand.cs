using System;
using Game.Data.FeatureOpen;
using Game.Data.GM;
using Game.GamePlaySystem.FeatureOpen;

namespace Game.GamePlaySystem.GM
{
    [GMAttr(type = "功能", name = "特定功能解锁", priority = 1)]
    public class OpenFeatureCommand : ICommand
    {
        public int featureId;
        
        public void Run()
        {
            FeatureOpenManager.Instance.OpenFeature((FeatureType)featureId);
        }
    }
    
    [GMAttr(type = "功能", name = "全功能解锁", priority = 2)]
    public class OpenAllFeatureCommand : ICommand
    {
        public void Run()
        {
            foreach (var item in Enum.GetValues(typeof(FeatureType)))
            {
                FeatureOpenManager.Instance.OpenFeature((FeatureType)item);
            }
        }
    }
}