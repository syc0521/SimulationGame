using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Game.Data.TableData;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class HouseConsumeAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/Configs/RawTable/BuildingTable.xlsx";
    private static readonly string assetFilePath = "Assets/Configs/RawTable/HouseConsume.asset";
    private static readonly string sheetName = "HouseConsume";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            HouseConsume data = (HouseConsume)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(HouseConsume));
            if (data == null) {
                data = ScriptableObject.CreateInstance<HouseConsume> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<HouseConsumeData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<HouseConsumeData>().ToArray();
                data.dataList = query.Deserialize<HouseConsumeData>();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
