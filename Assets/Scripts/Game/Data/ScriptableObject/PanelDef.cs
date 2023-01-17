using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Data.ScriptableObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PanelDef")]
    public class PanelDef : UnityEngine.ScriptableObject, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct PanelStruct
        {
            public PanelEnum panelEnum;
            public GameObject obj;
        }

        [SerializeField]
        private List<PanelStruct> _panelTemp;
        public Dictionary<PanelEnum, GameObject> panels = new();

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            foreach (var item in _panelTemp.Where(item => !panels.ContainsKey(item.panelEnum)))
            {
                panels[item.panelEnum] = item.obj;
            }
        }
    }
}