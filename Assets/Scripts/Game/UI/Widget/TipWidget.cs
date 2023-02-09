using TMPro;

namespace Game.UI.Widget
{
    public class TipWidget : WidgetBase
    {
        public TextMeshProUGUI title_txt, desc_txt;

        public void SetTitle(string text)
        {
            title_txt.text = text;
        }

        public void SetDescription(string text)
        {
            desc_txt.text = text;
        }
    }
}