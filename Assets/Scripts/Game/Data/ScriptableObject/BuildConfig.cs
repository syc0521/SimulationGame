using UnityEngine;

namespace Game.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/BuildConfig")]
    public class BuildConfig : UnityEngine.ScriptableObject
    {
        public int col, row;
    }
}