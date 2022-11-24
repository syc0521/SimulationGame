using UnityEngine;

namespace Game.GamePlaySystem
{
    public static class MaterialUtil
    {
        private static readonly int IsTransparent = Shader.PropertyToID("_IsTransparency");

        public static void SetTransparency(GameObject obj, bool isTransparent)
        {
            var meshList = obj.gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshList)
            {
                mesh.material.SetFloat(IsTransparent, isTransparent ? 1 : 0);
            }
        }
    }
}