using UnityEngine;

namespace Game.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GestureConfig")]
    public class GestureConfig : UnityEngine.ScriptableObject
    {
        public float cameraSpeed = 7.5f;
        public float minHeight = 2;
        public float maxHeight = 12;
        public Vector2 xLimit, zLimit;
    }
}