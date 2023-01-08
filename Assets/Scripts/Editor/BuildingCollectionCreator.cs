using System.Collections.Generic;
using System.Linq;
using Game.Data.ScriptableObject;
using Game.Data.TableData;
using Game.LevelAndEntity.ResLoader;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class BuildingCollectionCreator : MonoBehaviour
    {
        private static readonly string rootPath = "Assets/Res/entity/";

        [MenuItem("工具/导入建筑")]
        public static void ImportBuilding()
        {
            var data = (Building)AssetDatabase.LoadAssetAtPath("Assets/Configs/building.asset", typeof(Building));
            var buildingObjs = data.dataList.Select(buildingData => 
                (GameObject)AssetDatabase.LoadAssetAtPath(GetAssetPath(ResEnum.building, buildingData.Resourcepath), typeof(GameObject)));

            var collection = (BuildingCollection)AssetDatabase.LoadAssetAtPath("Assets/Configs/BuildingCollection.asset", typeof(BuildingCollection));
            collection.buildings.Clear();
            collection.buildings.AddRange(buildingObjs);

        }
        
        private static string GetAssetPath(ResEnum type, string path) => $"{rootPath}{type}/{path}/{path}.prefab";

    }
}