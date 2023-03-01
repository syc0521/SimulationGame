﻿using Game.Data;
using Game.Data.FeatureOpen;

namespace Game.UI.Panel.FeatureOpen
{
    public class FeatureOpenPanelOption : BasePanelOption
    {
        public FeatureType type;
    }
    
    public class FeatureOpenPanel : UIPanel
    {
        public FeatureOpenPanel_Nodes nodes;

        public override void OnCreated()
        {
            base.OnCreated();
            nodes.close_btn.onClick.AddListener(CloseSelf);
        }

        public override void OnShown()
        {
            base.OnShown();
            if (opt is not FeatureOpenPanelOption option) return;
            ShowFeature(option.type);
        }

        public override void OnDestroyed()
        {
            nodes.close_btn.onClick.RemoveListener(CloseSelf);
            base.OnDestroyed();
        }

        private void ShowFeature(FeatureType type)
        {
            var featureData = ConfigTable.Instance.GetFeatureOpenData(type);
            nodes.name_txt.text = featureData.Name;
            nodes.detail_txt.text = featureData.Description;
        }
    }
}