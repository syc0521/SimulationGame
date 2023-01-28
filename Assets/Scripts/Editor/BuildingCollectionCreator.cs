using System.Collections.Generic;
using System.Linq;
using Game.Data.ScriptableObject;
using Game.Data.TableData;
using Game.LevelAndEntity.Authoring;
using Game.LevelAndEntity.ResLoader;
using UnityEditor;
using UnityEngine;
public class BuildingCollectionCreator : MonoBehaviour
{
    private static readonly string rootPath = "Assets/Res/entity/";

#if UNITY_EDITOR
    
    [MenuItem("工具/导入建筑")]
    public static void ImportBuilding()
    {
        var data = (Building)AssetDatabase.LoadAssetAtPath("Assets/Configs/building.asset", typeof(Building));

        List<GameObject> buildingObjs = new();
        foreach (var buildingData in data.dataList)
        {
            var buildingObj = (GameObject)AssetDatabase.LoadAssetAtPath(
                GetAssetPath(ResEnum.building, buildingData.Resourcepath), typeof(GameObject));
            buildingObj.GetComponent<BuildingAuthoring>().type = buildingData.Buildingid;
            buildingObjs.Add(buildingObj);
        }
        
        var collection = (BuildingCollection)AssetDatabase.LoadAssetAtPath("Assets/Configs/BuildingCollection.asset", typeof(BuildingCollection));
        collection.buildings.Clear();
        collection.buildings.AddRange(buildingObjs);

        EditorUtility.SetDirty(collection);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
    
    private static string GetAssetPath(ResEnum type, string path) => $"{rootPath}{type}/{path}/{path}.prefab";

}