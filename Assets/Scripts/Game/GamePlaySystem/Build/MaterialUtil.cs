using Game.Core;
using Game.Data;
using Game.LevelAndEntity.ResLoader;
using UnityEngine;

namespace Game.GamePlaySystem.Build
{
    public static class MaterialUtil
    {
        public static void SetTransparency(GameObject obj)
        {
            var meshList = obj.gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshList)
            {
                var materials = new Material[mesh.materials.Length];
                for (var i = 0; i < mesh.materials.Length; i++)
                {
                    var mat = mesh.materials[i];
                    var name = mat.name.Replace(" (Instance)", string.Empty);
                    materials[i] = Managers.Get<IResLoader>().LoadMaterial(name);
                }
                mesh.materials = materials;
            }
        }
    }
}