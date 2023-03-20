/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using UnityEditor;
using System.Linq;

namespace CriWare.Assets {
	[CustomEditor(typeof(CriManaMovieControllerForAsset))]
	class CriManaMovieControllerForAssetEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CriManaMovieControllerForAsset.usmAsset)));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CriManaMovieControllerForAsset.playOnStart)));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CriManaMovieControllerForAsset.restartOnEnable)));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_material"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CriManaMovieControllerForAsset.renderMode)));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxFrameDrop"));

			var targetProp = serializedObject.FindProperty(nameof(CriManaMovieControllerForAsset.target));
			var index = CriEditorUtilitiesInternal.GetSubclassesOf(typeof(ICriManaMovieMaterialTarget)).Select(t => string.Format("{0} {1}", t.Assembly.ToString().Split(',')[0], t.FullName)).ToList().IndexOf(targetProp.managedReferenceFullTypename);
			var newindex = EditorGUILayout.Popup("Render Target", index, CriEditorUtilitiesInternal.GetSubclassesOf(typeof(ICriManaMovieMaterialTarget)).Select(t => t.Name.Replace("ManaMovieMaterial", "").Replace("Target", "")).ToArray());
			if (newindex != index)
			{
				index = newindex;
				targetProp.managedReferenceValue = System.Activator.CreateInstance(CriEditorUtilitiesInternal.GetSubclassesOf(typeof(ICriManaMovieMaterialTarget)).ToList()[index]);
			}

			EditorGUILayout.PropertyField(targetProp, true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CriManaMovieControllerForAsset.useOriginalMaterial)));

			serializedObject.ApplyModifiedProperties();
		}
	}
}

/** @} */
