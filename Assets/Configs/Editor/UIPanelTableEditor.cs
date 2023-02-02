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
[CustomEditor(typeof(UIPanelTable))]
public class UIPanelTableEditor : BaseExcelEditor<UIPanelTable>
{	    
    public override bool Load()
    {
        UIPanelTable targetData = target as UIPanelTable;

        string path = targetData.SheetName;
        if (!File.Exists(path))
            return false;

        string sheet = targetData.WorksheetName;

        ExcelQuery query = new ExcelQuery(path, sheet);
        if (query != null && query.IsValid())
        {
            targetData.dataArray = query.Deserialize<UIPanelTableData>().ToArray();
            targetData.dataList = query.Deserialize<UIPanelTableData>();
            EditorUtility.SetDirty(targetData);
            AssetDatabase.SaveAssets();
            return true;
        }
        else
            return false;
    }
}
