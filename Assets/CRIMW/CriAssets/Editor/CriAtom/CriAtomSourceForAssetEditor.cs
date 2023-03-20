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
	[CustomEditor(typeof(CriAtomSourceForAsset))]
	class CriAtomSourceForAssetEditor : CriAtomSourceBaseEditor
	{
		protected override void InspectorCueReferenceGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("cue"));
			serializedObject.ApplyModifiedProperties();

		}

		protected override void InspectorPreviewGUI()
		{
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Preview", GUILayout.MaxWidth(EditorGUIUtility.labelWidth - 5));
				if (GUILayout.Button("Play", GUILayout.MaxWidth(60)))
				{
					var cue = (source as CriAtomSourceForAsset).Cue;
					CriAtomAssetsPreviewPlayer.Instance.Play(cue.AcbAsset, cue.CueId);
				}
				if (GUILayout.Button("Stop", GUILayout.MaxWidth(60)))
				{
					CriAtomAssetsPreviewPlayer.Instance.Stop();
				}
			}
			GUILayout.EndHorizontal();
		}

		private void OnDisable()
		{
			CriAtomAssetsPreviewPlayer.Instance.Dispose();
		}
	}
}

/** @} */
