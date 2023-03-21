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
        }

        public override void OnShown()
        {
            base.OnShown();
            InitData();
            nodes.currentLevel_txt.text = $"{StringUtility.ConvertNumberToString(_level)}级";
            nodes.level_txt.text = $"{StringUtility.ConvertNumberToString(_level)}级";
            nodes.nextLevel_txt.text = $"{StringUtility.ConvertNumberToString(_level + 1)}级";
        }

        public override void OnDestroyed()
        {
            nodes.achievement_btn.onClick.RemoveListener(OpenAchievementPanel);
            nodes.upgrade_btn.onClick.RemoveListener(Upgrade);
            base.OnDestroyed();
        }

        private void InitData()
        {
            if (BuildingManager.Instance.TryGetStaticBuildingLevel(10001, out var level))
            {
                _level = level;
            }
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
    }
}