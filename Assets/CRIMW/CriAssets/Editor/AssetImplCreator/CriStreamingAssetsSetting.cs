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

namespace CriWare.Assets
{
	public class CriStreamingAssetsSetting : CriAssetSettingsBase<CriStreamingAssetsSetting>
	{
		[SerializeField]
		internal string deployFolderPath = "CriStreamingData";
	}

	[CustomEditor(typeof(CriStreamingAssetsSetting))]
	public class CriStreamingAssetsSettingEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("deployFolderPath"));
			EditorGUILayout.LabelField( $"StreamingAssets/{serializedObject.FindProperty("deployFolderPath").stringValue}/", EditorStyles.boldLabel);
			serializedObject.ApplyModifiedProperties();
		}
	}
}

/** @} */
