using UnityEngine;

namespace Game.UI.Component
{
    public class FrameComponent : MonoBehaviour
    {
        public void SetFrame(int index)
        {
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i + 1 == index);
            }
        }
    }
}