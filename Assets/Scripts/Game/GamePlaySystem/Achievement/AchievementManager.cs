using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Data;
using Game.Data.Achievement;
using Game.Data.Event;
using Unity.Mathematics;

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

            foreach (var data in ConfigTable.Instance.GetAchievement().dataList.Where(data => !achievementData.ContainsKey(data.ID)))
            {
                achievementData[data.ID] = new PlayerAchievementData
                {
                    complete = false,
                    progress = 0
                };
            }
            Managers.Get<ISaveDataManager>().SaveData();
        }

        public void TriggerAchievement(AchievementType type, int id, int count)
        {
            foreach (var (dataId, playerData) in achievementData)
            {
                if (playerData.complete) continue;
                
                var tableData = ConfigTable.Instance.GetAchievementData(dataId);
                if (tableData.Type != (int)type) continue;
                
                if ((AchievementType)tableData.Type is AchievementType.People) // 人口需要特殊处理
                {
                    playerData.progress = math.max(playerData.progress, count);
                }
                else if (tableData.Targetid == id)
                {
                    playerData.progress = math.min(playerData.progress + count, tableData.Targetnum);
                }

                if (playerData.progress >= tableData.Targetnum)
                {
                    playerData.complete = true;
                }
            }
        }

        public int GetTotalAchievement => ConfigTable.Instance.GetAchievement().dataList.Count;

        public int GetCompletedAchievement => achievementData.Count(item => item.Value.complete);

        public Dictionary<int, PlayerAchievementData> GetAchievementData => achievementData;
    }
}