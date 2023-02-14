using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using Game.Data.TableData;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class CurrencyAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/Configs/RawTable/Currency.xlsx";
    private static readonly string assetFilePath = "Assets/Configs/RawTable/Currency.asset";
    private static readonly string sheetName = "Currency";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            Currency data = (Currency)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Currency));
            if (data == null) {
                data = ScriptableObject.CreateInstance<Currency> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<CurrencyData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<CurrencyData>().ToArray();
                data.dataList = query.Deserialize<CurrencyData>();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
