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
using System.Globalization;
using System.Linq;

namespace CriWare.Assets
{
    [CustomEditor(typeof(CriAtomAcbLocalizedAsset))]
    class CriAtomAcbLocalizedAssetEditor : UnityEditor.Editor
    {
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("assets"));
			serializedObject.ApplyModifiedProperties();
		}
	}

	[CustomPropertyDrawer(typeof(CriAtomAcbLocalizedAsset.LanguageAssetPair))]
	class LanguageAssetPairDrawer : PropertyDrawer
	{
		internal static string[] Langs = new string[] {
			"ja",
			"en",
			"fr",
			"it",
			"de",
			"es",
			"ko",
			"pt",
			"zh",
		};

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position.width /= 2f;
			var langProp = property.FindPropertyRelative(nameof(CriAtomAcbLocalizedAsset.LanguageAssetPair.language));
			langProp.stringValue = Langs[EditorGUI.Popup(position, Mathf.Max(System.Array.IndexOf(Langs, langProp.stringValue), 0), Langs)];
			position.x += position.width;
			EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(CriAtomAcbLocalizedAsset.LanguageAssetPair.asset)), GUIContent.none);
		}
	}
}

/** @} */
