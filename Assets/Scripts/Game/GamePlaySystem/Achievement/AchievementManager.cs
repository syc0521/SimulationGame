using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.Data.Event;

namespace Game.GamePlaySystem.Achievement
{
    public class AchievementManager : GamePlaySystemBase<AchievementManager>
    {
        private Dictionary<int, PlayerAchievementData> achievementData;
        
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
            Managers.Get<ISaveDataManager>().GetPlayerAchievement(ref achievementData);
        }
        
        
    }
}