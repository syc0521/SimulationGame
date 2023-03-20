/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	[System.Serializable]
	class TargetAssetPairGeneric<T>
		where T : CriAssetBase
	{
		public BuildTarget target;
		public T asset;
	}

	abstract class CriMultiPlatformAssetImporter<T1, T2, T3> : ScriptedImporter
		where T1 : CriAssetBase
		where T2 : CriAssetBase, ICriReferenceAsset
		where T3 : TargetAssetPairGeneric<T1>
	{
		[SerializeField]
		T1 defaultAsset = null;
		[SerializeField]
		T3[] overrideAssets = null;

		public override void OnImportAsset(AssetImportContext ctx)
		{
			var instance = ScriptableObject.CreateInstance<T2>();
			var serialized = new SerializedObject(instance);
			serialized.FindProperty("original").objectReferenceValue = GetCurrentAsset(ctx);
			serialized.ApplyModifiedPropertiesWithoutUndo();
			ctx.AddObjectToAsset("main", instance);
			ctx.SetMainObject(instance);
		}

		T1 GetCurrentAsset(AssetImportContext ctx)
		{
			if (overrideAssets != null)
				foreach (var pair in overrideAssets)
					if (pair.target == ctx.selectedBuildTarget)
						return pair.asset;
			return defaultAsset;
		}

		protected static void CreateFile(string ext)
		{
			ProjectWindowUtil.CreateAssetWithContent($"New {typeof(T2).Name}.{ext}", "");
			AssetDatabase.Refresh();
		}
	}

	[CustomPropertyDrawer(typeof(TargetAssetPairGeneric<>), true)]
	class TargetAssetPairDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position.width /= 2;
			EditorGUI.PropertyField(position, property.FindPropertyRelative("target"), GUIContent.none);
			position.x += position.width;
			EditorGUI.PropertyField(position, property.FindPropertyRelative("asset"), GUIContent.none);
		}
	}
}

/** @} */
