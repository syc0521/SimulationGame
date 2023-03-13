using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Data.TableData
{
    ///
    /// !!! Machine generated code !!!
    ///
    /// A class which deriveds ScritableObject class so all its data 
    /// can be serialized onto an asset data file.
    /// 
    [System.Serializable]
    public class BuildingUpgrade : UnityEngine.ScriptableObject 
    {	
        [HideInInspector] [SerializeField] 
        public string SheetName = "";
        
        [HideInInspector] [SerializeField] 
        public string WorksheetName = "";
        
        // Note: initialize in OnEnable() not here.
        public BuildingUpgradeData[] dataArray;
        public List<BuildingUpgradeData> dataList;
        
        void OnEnable()
        {		
    //#if UNITY_EDITOR
            //hideFlags = HideFlags.DontSave;
    //#endif
            // Important:
            //    It should be checked an initialization of any collection data before it is initialized.
            //    Without this check, the array collection which already has its data get to be null 
            //    because OnEnable is called whenever Unity builds.
            // 		
            dataArray ??= Array.Empty<BuildingUpgradeData>();

        }
        
        //
        // Highly recommand to use LINQ to query the data sources.
        //

    }
}
