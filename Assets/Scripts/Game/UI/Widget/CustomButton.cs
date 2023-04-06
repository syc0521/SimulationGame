using Game.Audio;
using Game.Core;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Widget
{
    public class CustomButton : Button
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Managers.Get<IAudioManager>().PlaySFX(SFXType.Button);
        }
    }
}