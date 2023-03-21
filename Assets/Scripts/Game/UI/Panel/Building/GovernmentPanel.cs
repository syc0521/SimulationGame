using Game.GamePlaySystem;
using Game.UI.Panel.Achievement;
using Game.UI.Utils;

namespace Game.UI.Panel.Building
{
    public class GovernmentPanel : UIPanel
    {
        public GovernmentPanel_Nodes nodes;
        private int _level;

        public override void OnCreated()
        {
            base.OnCreated();
            nodes.achievement_btn.onClick.AddListener(OpenAchievementPanel);
            nodes.upgrade_btn.onClick.AddListener(Upgrade);
            nodes.back_btn.onClick.AddListener(CloseSelf);
        }

        public override void OnShown()
        {
            base.OnShown();
            InitData();
        }

        public override void OnDestroyed()
        {
            nodes.achievement_btn.onClick.RemoveListener(OpenAchievementPanel);
            nodes.upgrade_btn.onClick.RemoveListener(Upgrade);
            nodes.back_btn.onClick.RemoveAllListeners();
            base.OnDestroyed();
        }

        private void InitData()
        {
            if (BuildingManager.Instance.TryGetStaticBuildingLevel(10001, out var level))
            {
                _level = level;
            }
            nodes.currentLevel_txt.text = $"{StringUtility.ConvertNumberToString(_level)}级";
            nodes.level_txt.text = $"{StringUtility.ConvertNumberToString(_level)}级";
            nodes.nextLevel_txt.text = $"{StringUtility.ConvertNumberToString(_level + 1)}级";
            var data = SystemDataManager.Instance.GetGameData();
            if (data.Equals(default))
            {
                return;
            }

            nodes.cityEnv_txt.text = GetEnvRateString(data.environment);
            nodes.cityState_txt.text = GetBuildingRateString(data.buildingRate);
        }

        private void RenderList()
        {
            
        }

        private void OpenAchievementPanel()
        {
            UIManager.Instance.OpenPanel<AchievementPanel>();
        }

        private void Upgrade()
        {
            BuildingManager.Instance.UpgradeBuilding(10001, _level + 1, true);
        }

        private string GetEnvRateString(float rate)
        {
            return rate switch
            {
                >= 0.8f => "优美",
                >= 0.6f => "一般",
                _ => "较差"
            };
        }
        
        private string GetBuildingRateString(float rate)
        {
            return rate switch
            {
                >= 0.8f => "繁荣",
                >= 0.5f => "较好",
                _ => "破败"
            };
        }
    }
}