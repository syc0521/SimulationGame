using Game.UI.Widget;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI.Panel.Loading
{
    public class LoadingPanel_Nodes : MonoBehaviour
    {
        public CustomSliderWidget slider;
        [FormerlySerializedAs("progress_txt")] public TextMeshProUGUI tip_txt;
    }
}