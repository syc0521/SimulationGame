using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Rendering.Occlusion
{
    public class Occluder : MonoBehaviour
    {
        [FormerlySerializedAs("Mesh")] public Mesh mesh;

        [FormerlySerializedAs("relativePosition")]
        public Vector3 localPosition = Vector3.zero;

        [FormerlySerializedAs("relativeRotation")]
        public Quaternion localRotation = Quaternion.identity;

        [FormerlySerializedAs("relativeScale")]
        public Vector3 localScale = Vector3.one;

#if ENABLE_UNITY_OCCLUSION && (HDRP_10_0_0_OR_NEWER || URP_10_0_0_OR_NEWER)
        void Reset()
        {
            if (gameObject.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                mesh = meshFilter.sharedMesh;
            }

            localPosition = Vector3.zero;
            localRotation = Quaternion.identity;
            localScale = Vector3.one;
        }

        private void OnDrawGizmos()
        {
            DrawGizmos(false);
        }

        private void OnDrawGizmosSelected()
        {
            DrawGizmos(true);
        }

        private void DrawGizmos(bool selected)
        {
            if (mesh == null || mesh.vertexCount == 0)
            {
                return;
            }

            Gizmos.color = selected ? Color.yellow : Color.white;
            Matrix4x4 mtx = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.TRS(localPosition, localRotation, localScale);
            Gizmos.DrawWireMesh(mesh);
            Gizmos.matrix = mtx;
        }
#endif
    }
}

