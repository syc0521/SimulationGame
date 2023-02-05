using UnityEngine;

namespace Game.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GestureConfig")]
    public class GestureConfig : UnityEngine.ScriptableObject
    {
        public float cameraSpeed = 7.5f;
    }
}