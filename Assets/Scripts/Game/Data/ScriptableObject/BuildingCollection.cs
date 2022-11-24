using System.Collections.Generic;
using UnityEngine;

namespace Game.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/BuildingCollection")]
    public class BuildingCollection : UnityEngine.ScriptableObject
    {
        public List<GameObject> buildings;
    }
}