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
        Building = 0,
        Sprite = 1,
        Panel = 2,
        Material = 3,
    }

    public class ResLoader : ManagerBase, IResLoader
    {
        private Dictionary<string, Material> _materials = new();

        public override void OnStart()
        {
            base.OnStart();
            LoadAllMaterials();
        }

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

        public Material LoadMaterial(string name)
        {
            return _materials.ContainsKey(name) ? _materials[name] : null;
        }

        private void LoadAllMaterials()
        {
            Addressables.LoadAssetsAsync<Material>("Material", result =>
            {
                _materials[result.name.Replace(" Variant", string.Empty)] = result;
            });
        }

        public bool UnloadRes(ResEnum type, string path)
        {
            return false;
        }

        private string GetAssetPath(ResEnum type, string path)
        {
            switch (type)
            {
                case ResEnum.Building:
                    return $"{path}/{path}.prefab";
                case ResEnum.Sprite:
                    return $"{path}.png";
                case ResEnum.Panel:
                    return $"panel/{path}.prefab";
                case ResEnum.Material:
                    return $"material/{path} Variant.mat";
                default:
                    return string.Empty;
            }
        }
    }
}