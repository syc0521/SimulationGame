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
        Picture = 4,
    }

    public class ResLoader : ManagerBase, IResLoader
    {
        private Dictionary<string, Material> _materials = new();
        private Dictionary<(ResEnum, string), AsyncOperationHandle> _handles = new();

        public override void OnStart()
        {
            base.OnStart();
            LoadAllMaterials();
        }

        public void LoadRes(ResEnum type, string path, Action<AsyncOperationHandle<GameObject>> callback)
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(GetAssetPath(type, path));
                _handles[(type, path)] = handle;
                handle.Completed += callback;
            }
            catch (Exception)
            {
                Debug.LogError($"获取{path}资源错误！");
            }
        }
        
        public void LoadRes(ResEnum type, string path, Action<AsyncOperationHandle<Texture2D>> callback)
        {
            try
            {
                var handle = Addressables.LoadAssetAsync<Texture2D>(GetAssetPath(type, path));
                _handles[(type, path)] = handle;
                handle.Completed += callback;
            }
            catch (Exception)
            {
                Debug.LogError($"获取{path}资源错误！");
            }
        }

        public Material LoadMaterial(string name)
        {
            return _materials.TryGetValue(name, out var material) ? material : null;
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
            Addressables.ReleaseInstance(_handles[(type, path)]);
            _handles.Remove((type, path));
            return true;
        }

        public void UnloadRes(AsyncOperationHandle handle)
        {
            Addressables.ReleaseInstance(handle);
        }

        private string GetAssetPath(ResEnum type, string path)
        {
            return type switch
            {
                ResEnum.Building => $"{path}/{path}.prefab",
                ResEnum.Sprite => $"{path}.png",
                ResEnum.Picture => $"picture/{path}.png",
                ResEnum.Panel => $"panel/{path}.prefab",
                ResEnum.Material => $"material/{path} Variant.mat",
                _ => string.Empty
            };
        }
    }
}