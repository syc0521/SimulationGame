using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Core;
using Game.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.LevelAndEntity.ResLoader
{
    public enum ResEnum
    {
        building = 0,
        sprite = 1,
    }

    public class ResLoader : ManagerBase, IResLoader
    {
        private readonly string rootPath = "Assets/Res";

        public void LoadRes(ResEnum type, string path, Action<AsyncOperationHandle<GameObject>> callback)
        {
            try
            {
                Addressables.LoadAssetAsync<GameObject>(GetAssetPath(type, path)).Completed += callback;
            }
            catch (Exception e)
            {
                Debug.LogError($"获取{path}资源错误！");
            }
        }
        
        public void LoadRes(ResEnum type, string path, Action<AsyncOperationHandle<Texture2D>> callback)
        {
            try
            {
                Addressables.LoadAssetAsync<Texture2D>(GetAssetPath(type, path)).Completed += callback;
            }
            catch (Exception e)
            {
                Debug.LogError($"获取{path}资源错误！");
            }
        }

        public bool UnloadRes(ResEnum type, string path)
        {
            return false;
        }

        private string GetAssetPath(ResEnum type, string path)
        {
            switch (type)
            {
                case ResEnum.building:
                    return $"{rootPath}/{type}/{path}/{path}.prefab";
                case ResEnum.sprite:
                    return $"{rootPath}/ui/dynamic/{path}";
                default:
                    return string.Empty;
            }
        }
    }
}