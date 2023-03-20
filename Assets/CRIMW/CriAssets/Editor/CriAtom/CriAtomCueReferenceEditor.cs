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
using System.Linq;

namespace CriWare.Assets
{
	[CustomPropertyDrawer(typeof(CriAtomCueReference))]
	class CriAtomCueReferenceEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!CriAtomPlugin.IsLibraryInitialized())
				CriWare.Editor.CriAtomEditorUtilities.InitializeLibrary();

			position.width /= 2f;
			EditorGUI.PropertyField(position, property.FindPropertyRelative("acbAsset"), new GUIContent(label));

			position.x += position.width;

			var acb = (CriAtomAcbAsset)property.FindPropertyRelative("acbAsset").objectReferenceValue;

			if (acb == null) return;

			var handle = CriAtomAssetsPreviewPlayer.Instance.GetAcb(acb);
			if (handle == null) return;

			EditorGUI.IntPopup(position,
				property.FindPropertyRelative("cueId"),
				handle.GetCueInfoList().Select(info => new GUIContent(info.id.ToString("000") + ":" + info.name)).ToArray(),
				handle.GetCueInfoList().Select(info => info.id).ToArray(),
				GUIContent.none
				);
		}
	}
}

/** @} */
