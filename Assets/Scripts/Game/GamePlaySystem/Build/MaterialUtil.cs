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
                    materials[i].color = new Color(1, 1, 1, 170 / 255f);
                }
                mesh.materials = materials;
            }
        }

        public static void SetColor(GameObject obj, bool canConstruct = true)
        {
            var meshList = obj.gameObject.GetComponentsInChildren<MeshRenderer>();
            var errorColor = new Color(245 / 255f, 36 / 255f, 36 / 255f, 170 / 255f);
            var normalColor = new Color(1, 1, 1, 170 / 255f);
            foreach (var mesh in meshList)
            {
                foreach (var mat in mesh.materials)
                {
                    mat.color = canConstruct ? normalColor : errorColor;
                }
            }
        }
    }
}