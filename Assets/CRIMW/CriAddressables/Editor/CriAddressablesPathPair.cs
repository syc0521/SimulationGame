/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ADDRESSABLES_INTEGRATION
 * @{
 */

#if CRI_USE_ADDRESSABLES

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.Security.Cryptography;
using System;
using System.Linq;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace CriWare.Assets
{
	[CreateAssetMenu(fileName = "NewCriAddressableGroup", menuName = "CRIWARE/Cri Addressable Group")]
	internal class CriAddressablesPathPair : ScriptableObject
	{
		[SerializeField]
		public ProfileValueReference buildPath;
		[SerializeField]
		public ProfileValueReference loadPath;

		internal CriAddressableGroup CreateGroup() => new CriAddressableGroup($"CriData_{this.name}", "CriPackedAssetsTemplate", this);
	}

	[CustomEditor(typeof(CriAddressablesPathPair))]
	internal class CriAddressablesPathPairEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("buildPath"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("loadPath"));
			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(target);
				AssetDatabase.SaveAssets();
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}

#endif

/** @} */
