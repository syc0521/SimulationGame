﻿using System;
using Game.UI.Component;
using TMPro;
using UnityEngine.UI;

namespace Game.UI.Widget
{
    public class BuildingDetailWidget : WidgetBase
    {
        public TextMeshProUGUI name_txt, desc_txt;
        public TextMeshProUGUI level_txt, produce_txt;
        public FrameComponent upgrade_frame;
        public CustomButton upgrade_btn;
        private Action _handler;

        public override void OnCreated()
        {
            base.OnCreated();
            upgrade_btn.onClick.AddListener(ClickHandler);
        }

        public override void OnDestroyed()
        {
            upgrade_btn.onClick.RemoveListener(ClickHandler);
            base.OnDestroyed();
        }

        public void SetDefault()
        {
            name_txt.text = string.Empty;
            desc_txt.text = string.Empty;
            level_txt.gameObject.SetActive(false);
            level_txt.text = string.Empty;
            produce_txt.gameObject.SetActive(false);
            produce_txt.text = string.Empty;
            upgrade_frame.gameObject.SetActive(false);
        }

        public void SetTitle(string text)
        {
            name_txt.text = text;
        }

        public void SetDescription(string text)
        {
            desc_txt.text = text;
        }

        public void SetLevel(int level)
        {
            level_txt.gameObject.SetActive(true);
            level_txt.text = $"Lv.{level}";
        }

        public void SetProduceAmount(int amount)
        {
            produce_txt.gameObject.SetActive(true);
            produce_txt.text = $"{amount}/分钟";
        }

        public void SetUpgradeState(bool canUpgrade)
        {
            upgrade_frame.gameObject.SetActive(true);
            upgrade_frame.SetFrame(canUpgrade ? 1 : 2);
        }

        public void SetUpgradeHandler(Action handler)
        {
            _handler = handler;
        }

        private void ClickHandler()
        {
            _handler?.Invoke();
        }
    }
}