using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Game.Data.TableData;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class FeatureOpenAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/Configs/RawTable/FeatureOpen.xlsx";
    private static readonly string assetFilePath = "Assets/Configs/RawTable/FeatureOpen.asset";
    private static readonly string sheetName = "FeatureOpen";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            FeatureOpen data = (FeatureOpen)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(FeatureOpen));
            if (data == null) {
                data = ScriptableObject.CreateInstance<FeatureOpen> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<FeatureOpenData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<FeatureOpenData>().ToArray();
                data.dataList = query.Deserialize<FeatureOpenData>();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
