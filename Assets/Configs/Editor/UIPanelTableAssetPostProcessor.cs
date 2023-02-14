using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Game.Data.TableData;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class UIPanelTableAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/Configs/RawTable/UIPanel.xlsx";
    private static readonly string assetFilePath = "Assets/Configs/RawTable/UIPanelTable.asset";
    private static readonly string sheetName = "UIPanelTable";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            UIPanelTable data = (UIPanelTable)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(UIPanelTable));
            if (data == null) {
                data = ScriptableObject.CreateInstance<UIPanelTable> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<UIPanelTableData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<UIPanelTableData>().ToArray();
                data.dataList = query.Deserialize<UIPanelTableData>();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
