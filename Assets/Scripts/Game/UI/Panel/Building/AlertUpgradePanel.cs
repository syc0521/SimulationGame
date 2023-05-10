using Game.Data;
using Game.UI.Utils;

namespace Game.UI.Panel.Building
{
    public class AlertUpgradePanelOption : BasePanelOption
    {
        public int staticId;
        public int currentLevel;
        public bool isGov;
    }
    
    public class AlertUpgradePanel : UIPanel
    {
        public AlertUpgradePanel_Nodes nodes;
        public override void OnCreated()
        {
            base.OnCreated();
            nodes.close_btn.onClick.AddListener(CloseSelf);
        }

        public override void OnShown()
        {
            base.OnShown();
            ShowUpgradeInfo();
        }

        public override void OnDestroyed()
        {
            nodes.close_btn.onClick.RemoveListener(CloseSelf);
            base.OnDestroyed();
        }

        private void ShowUpgradeInfo()
        {
            if (opt is not AlertUpgradePanelOption option) return;

            var buildingData = ConfigTable.Instance.GetBuildingData(option.staticId);
            nodes.title_txt.text = buildingData.Name;
            nodes.currentLevel_txt.text = $"{StringUtility.ConvertNumberToString(option.currentLevel)}级";
            nodes.nextLevel_txt.text = $"{StringUtility.ConvertNumberToString(option.currentLevel + 1)}级";

            if (!option.isGov)
            {
                var produceData = ConfigTable.Instance.GetBuildingProduceData(option.staticId);
                var curItemPerMin = produceData.Produceamount[option.currentLevel - 1] / buildingData.Cd * 60.0f;
                var nextItemPerMin = produceData.Produceamount[option.currentLevel] / buildingData.Cd * 60.0f;
                nodes.currentProduction_txt.text = $"{(int)curItemPerMin}/分钟";
                nodes.nextProduction_txt.text = $"{(int)nextItemPerMin}/分钟";
            }
        }
        
    }
}