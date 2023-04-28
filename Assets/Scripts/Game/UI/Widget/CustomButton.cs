using Game.Audio;
using Game.Core;
using Game.UI.Component;
using Game.UI.Panel.GM;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Widget
{
    public class CustomButton : Button, IListWidget
    {
        private TextMeshProUGUI _text;
        
        protected override void Awake()
        {
            base.Awake();
            _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Managers.Get<IAudioManager>().PlaySFX(SFXType.Button);
        }

        public void SetText(string text)
        {
            if (_text != null)
            {
                _text.text = text;
            }
        }

        public void Refresh(ListData data)
        {
            if (data is GMCategoryListData categoryListData)
            {
                SetText(categoryListData.name);
                onClick.RemoveAllListeners();
                onClick.AddListener(() =>
                {
                    categoryListData.callback.Invoke(categoryListData.name, categoryListData.index);
                });
            }
        }
    }
}