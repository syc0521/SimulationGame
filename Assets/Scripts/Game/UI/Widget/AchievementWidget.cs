using Game.UI.Component;
using Game.UI.Panel.Achievement;
using TMPro;

namespace Game.UI.Widget
{
    public class AchievementWidget : WidgetBase, IListWidget
    {
        public TextMeshProUGUI title_txt;
        public CustomSliderWidget slider;
        public void Refresh(ListData data)
        {
            if (data is AchievementListData listData)
            {
                title_txt.text = listData.name;
                slider.SetDetailText(listData.desc);
                slider.SetAmountText(listData.complete ? "已完成" :$"{listData.current}/{listData.amount}");

                if (listData.amount == -1)
                {
                    slider.SetValue(listData.complete ? 1 : 0);
                }
                else
                {
                    slider.SetValue((float)listData.current / listData.amount);
                }
            }
        }
    }
}