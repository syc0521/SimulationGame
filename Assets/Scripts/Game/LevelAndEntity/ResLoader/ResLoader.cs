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
    }

    public class ResLoader : ManagerBase, IResLoader
    {
        private readonly string rootPath = "Assets/Res/";

        public void LoadRes(ResEnum type, string path, Action<AsyncOperationHandle<GameObject>> callback)
        {
            Addressables.LoadAssetAsync<GameObject>(GetAssetPath(type, path)).Completed += callback;
        }

        public bool UnloadRes(ResEnum type, string path)
        {
            return false;
        }

        private string GetAssetPath(ResEnum type, string path) => $"{rootPath}{type}/{path}/{path}.prefab";
    }
}