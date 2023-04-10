using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityQuickSheet;
using System.Linq;
using Game.Data.TableData;

///
/// !!! Machine generated code !!!
///
[CustomEditor(typeof(StoreItem))]
public class StoreItemEditor : BaseExcelEditor<StoreItem>
{	    
    public override bool Load()
    {
        StoreItem targetData = target as StoreItem;

        string path = targetData.SheetName;
        if (!File.Exists(path))
            return false;

        string sheet = targetData.WorksheetName;

        ExcelQuery query = new ExcelQuery(path, sheet);
        if (query != null && query.IsValid())
        {
            targetData.dataArray = query.Deserialize<StoreItemData>().ToArray();
            targetData.dataList = query.Deserialize<StoreItemData>();
            EditorUtility.SetDirty(targetData);
            AssetDatabase.SaveAssets();
            return true;
        }
        else
            return false;
    }
}
