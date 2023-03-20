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
using UnityEditorInternal;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	[CustomEditor(typeof(CriAtomAcbLocalizedAssetImporter))]
	class CriAtomAcbLocalizedAssetImporterEditor : ScriptedImporterEditor
	{
		const float langFieldWidth = 70;

		Vector2 scroolPos = Vector2.zero;

		ReorderableList _reorderableList = null;
		ReorderableList ReorderableList { get
			{
				if(_reorderableList == null)
				{
					_reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("assetsTable"));

					_reorderableList.drawElementCallback += (rect, index, active, focus) =>
						EditorGUI.PropertyField(rect, _reorderableList.serializedProperty.GetArrayElementAtIndex(index));

					_reorderableList.drawHeaderCallback += rect => {
						var platforms = serializedObject.FindProperty("platforms");

						if(platforms.arraySize <= 0)
						{
							GUI.Label(rect, "Assets");
							return;
						}

						using (new GUILayout.HorizontalScope())
						{
							rect.width -= 16;
							rect.x += 15;
							rect.x += langFieldWidth;
							rect.width -= langFieldWidth;
							rect.width /= platforms.arraySize + 1;
							GUI.Label(rect, "Default");
							for (int i = 0; i < platforms.arraySize; i++)
							{
								rect.x += rect.width;

								var popupRect = rect;
								popupRect.width -= CriEditorUtilitiesInternal.SingleReturnHeight;
								var prop = platforms.GetArrayElementAtIndex(i);
								EditorGUI.PropertyField(popupRect, prop, GUIContent.none);

								var buttonRect = rect;
								buttonRect.x += buttonRect.width;
								buttonRect.width = CriEditorUtilitiesInternal.SingleReturnHeight;
								buttonRect.x -= CriEditorUtilitiesInternal.SingleReturnHeight;
								if(GUI.Button(buttonRect, "X"))
								{
									platforms.DeleteArrayElementAtIndex(i);
									for (int j = 0; j < _reorderableList.serializedProperty.arraySize; j++)
									{
										var assetsProp = _reorderableList.serializedProperty.GetArrayElementAtIndex(j).FindPropertyRelative("assets");
										assetsProp.GetArrayElementAtIndex(i + 1).objectReferenceValue = null;
										assetsProp.DeleteArrayElementAtIndex(i + 1);
									}
									return;
								}
							}

						}
					};

					_reorderableList.onAddCallback += list => {
						_reorderableList.serializedProperty.InsertArrayElementAtIndex(_reorderableList.serializedProperty.arraySize);
						_reorderableList.serializedProperty.GetArrayElementAtIndex(_reorderableList.serializedProperty.arraySize - 1).FindPropertyRelative("assets").arraySize = serializedObject.FindProperty("platforms").arraySize + 1;
					};
				}
				return _reorderableList;
			} }

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			using(var scrollView = new EditorGUILayout.ScrollViewScope(scroolPos, GUI.skin.horizontalScrollbar, GUIStyle.none))
			{
				scroolPos = scrollView.scrollPosition;
				ReorderableList.DoLayoutList();
				using(new GUILayout.HorizontalScope())
					GUILayout.Space(langFieldWidth + (serializedObject.FindProperty("platforms").arraySize + 1) * 100f);
			}

			if(GUILayout.Button("Add Platform"))
			{
				serializedObject.FindProperty("platforms").InsertArrayElementAtIndex(0);
				for (int j = 0; j < _reorderableList.serializedProperty.arraySize; j++)
					_reorderableList.serializedProperty.GetArrayElementAtIndex(j).FindPropertyRelative("assets").InsertArrayElementAtIndex(0);
			}

			serializedObject.ApplyModifiedProperties();

			ApplyRevertGUI();
		}
	}

	[CustomPropertyDrawer(typeof(CriAtomAcbLocalizedAssetImporter.LanguageAssetsPair))]
	class LanguageAssetsPairDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using (new GUILayout.HorizontalScope())
			{
				var langProp = property.FindPropertyRelative("language");
				var assetsProp = property.FindPropertyRelative("assets");

				var langRect = position;
				langRect.width = 70;
				position.width -= langRect.width;
				position.x += langRect.width;
				langProp.stringValue = LanguageAssetPairDrawer.Langs[EditorGUI.Popup(langRect, Mathf.Max(System.Array.IndexOf(LanguageAssetPairDrawer.Langs, langProp.stringValue), 0), LanguageAssetPairDrawer.Langs)];

				position.width /= assetsProp.arraySize;

				for (int j = 0; j < assetsProp.arraySize; j++)
				{
					EditorGUI.PropertyField(position, assetsProp.GetArrayElementAtIndex(j), GUIContent.none);
					position.x += position.width;
				}
			}
		}
	}
}

/** @} */
