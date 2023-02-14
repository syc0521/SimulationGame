using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Game.Data.TableData;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class RewardGroupAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/Configs/RawTable/Task.xlsx";
    private static readonly string assetFilePath = "Assets/Configs/RawTable/RewardGroup.asset";
    private static readonly string sheetName = "RewardGroup";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            RewardGroup data = (RewardGroup)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(RewardGroup));
            if (data == null) {
                data = ScriptableObject.CreateInstance<RewardGroup> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<RewardGroupData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<RewardGroupData>().ToArray();
                data.dataList = query.Deserialize<RewardGroupData>();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
