using System;
using Game.Data.TableData;
using Game.UI.Component;
using Game.UI.ViewData;
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
        public ListComponent currency_list;
        private Action _handler;

        public override void OnCreated()
        {
            base.OnCreated();
            upgrade_btn.onClick.AddListener(ClickHandler);
            currency_list.Init();
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
            currency_list.Clear();
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

        public void SetCurrency(BuildingUpgradeData data)
        {
            for (var i = 0; i < data.Itemid.Length; i++)
            {
                var item = data.Itemid[i];
                if (item >= 0)
                {
                    currency_list.AddItem(new ConsumeItemListData
                    {
                        id = item,
                        amount = data.Itemcount[i],
                        consumeType = ConsumeType.Item,
                    });
                }
            }
            
            for (var i = 0; i < data.Currencyid.Length; i++)
            {
                var item = data.Currencyid[i];
                if (item >= 0)
                {
                    currency_list.AddItem(new ConsumeItemListData
                    {
                        id = item,
                        amount = data.Currencycount[i],
                        consumeType = ConsumeType.Currency,
                    });
                }
            }
        }

        private void ClickHandler()
        {
            _handler?.Invoke();
        }
    }
}