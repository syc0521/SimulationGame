using Game.UI.Component;
using Game.UI.Panel.GM;
using TMPro;

namespace Game.UI.Widget
{
    public class GMInfoWidget : WidgetBase, IListWidget
    {
        public TextMeshProUGUI _text;
        public TMP_InputField _inputfield;
        private int _index;

        public void SetText(string t)
        {
            _text.text = t;
        }

        public string GetData()
        {
            return _inputfield.text;
        }

        public int GetIndex()
        {
            return _index;
        }

        public void Refresh(ListData data)
        {
            if (data is GMInfoListData infoListData)
            {
                SetText(infoListData.name);
                _index = infoListData.index;
            }
        }
    }
}