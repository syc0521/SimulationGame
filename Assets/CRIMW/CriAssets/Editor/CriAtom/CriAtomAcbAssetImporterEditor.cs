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
	[CustomEditor(typeof(CriAtomAcbAssetImporter)), CanEditMultipleObjects]
	class CriAtomAcbAssetImporterEditor : CriAssetImporterEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var asset = AssetDatabase.LoadAssetAtPath<CriAtomAcbAsset>((target as CriAssetImporter).assetPath);
			if (asset == null) return;
			var handle = CriAtomAssetsPreviewPlayer.Instance.GetAcb(asset);
			if (handle == null) return;

			if (GUILayout.Button("Stop"))
				CriAtomAssetsPreviewPlayer.Instance.Stop();

			foreach (var cue in handle.GetCueInfoList())
			{
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button(EditorGUIUtility.IconContent("Animation.Play"), EditorStyles.miniButton, GUILayout.Width(30)))
				{
					CriAtomAssetsPreviewPlayer.Instance.Play(asset, cue.id);
				}
				EditorGUILayout.LabelField($"{cue.id: 000} : {cue.name}");
				EditorGUILayout.EndHorizontal();
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();

			CriAtomAssetsPreviewPlayer.Instance.Dispose();
		}
	}
}

/** @} */
