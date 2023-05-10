using Game.Data;
using Game.Data.FeatureOpen;
using Game.Data.TableData;
using Game.GamePlaySystem;
using Game.GamePlaySystem.Build;
using Game.GamePlaySystem.FeatureOpen;
using Game.UI.Decorator;
using Game.UI.Panel.Achievement;
using Game.UI.UISystem;
using Game.UI.Utils;
using Game.UI.ViewData;
using Game.UI.Widget;

namespace Game.UI.Panel.Building
{
    public class GovernmentPanel : UIPanel
    {
        public GovernmentPanel_Nodes nodes;
        private int _level;
        private const int EntityId = 10001, StaticId = 1;
        private BuildingUpgradeData _upgradeData;

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
            nodes.achievement_go.SetActive(FeatureOpenManager.Instance.HasFeature(FeatureType.Achievement));
            InitData();
            var maxLevel = ConfigTable.Instance.GetBuildingData(StaticId).Level;
            var isMaxLevel = _level >= maxLevel;
            nodes.upgrade_frame.SetFrame(isMaxLevel ? 2 : 1);
            if (!isMaxLevel)
            {
                RenderList();
            }
        }

        public override void OnDestroyed()
        {
            nodes.achievement_btn.onClick.RemoveListener(OpenAchievementPanel);
            nodes.upgrade_btn.onClick.RemoveListener(Upgrade);
            nodes.back_btn.onClick.RemoveListener(CloseSelf);
            base.OnDestroyed();
        }

        private void InitData()
        {
            if (BuildingManager.Instance.TryGetStaticBuildingLevel(EntityId, out var level))
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
            _upgradeData = ConfigTable.Instance.GetBuildingUpgradeData(StaticId, _level + 1);
        }

        private void RenderList()
        {
            for (var i = 0; i < _upgradeData.Itemid.Length; i++)
            {
                nodes.upgradeItem_list.AddItem(new ConsumeItemListData
                {
                    consumeType = ConsumeType.Item,
                    id = _upgradeData.Itemid[i],
                    amount = _upgradeData.Itemcount[i]
                });
            }
            for (var i = 0; i < _upgradeData.Currencyid.Length; i++)
            {
                nodes.upgradeItem_list.AddItem(new ConsumeItemListData
                {
                    consumeType = ConsumeType.Currency,
                    id = _upgradeData.Currencyid[i],
                    amount = _upgradeData.Currencycount[i]
                });
            }
        }

        private void OpenAchievementPanel()
        {
            UIManager.Instance.OpenPanel<AchievementPanel>();
        }

        private void Upgrade()
        {
            if (!BuildingSystem.Instance.UpgradeBuilding(EntityId, true))
            {
                AlertDecorator.OpenAlertPanel("货币或材料不足！", false);
            }
            else
            {
                AlertDecorator.OpenAlertUpgradePanel(StaticId, _level, true);
            }

            CloseSelf();
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
                >= 0.9f => "繁荣",
                >= 0.7f => "较好",
                >= 0.4f => "一般",
                _ => "破败"
            };
        }
    }
}