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
using System.Linq;
using System.Runtime.InteropServices;

namespace CriWare.Assets
{
	[CustomEditor(typeof(CriAtomAssets))]
	class CriAtomAssetsEditor : UnityEditor.Editor
	{
		ReorderableList _reorderableList = null;
		ReorderableList ReorderableList { get {
				if(_reorderableList == null)
				{
					_reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("acbAssets"));
					_reorderableList.drawElementCallback += (rect, index, active, focus) => EditorGUI.PropertyField(rect, serializedObject.FindProperty("acbAssets").GetArrayElementAtIndex(index));
					_reorderableList.drawHeaderCallback += rect => GUI.Label(rect, "Acb Assets");
				}
				return _reorderableList;
			} }

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
			{
				var acfProp = serializedObject.FindProperty("acfAsset");
				EditorGUILayout.PropertyField(acfProp);

				var acfAsset = acfProp.objectReferenceValue as CriAtomAcfAsset;
				if (acfAsset != null)
				{
					EditorGUI.indentLevel++;

					var busSettingProp = serializedObject.FindProperty("dspBusSetting");
					if (EditorApplication.isPlayingOrWillChangePlaymode)
					{
						EditorGUILayout.PropertyField(busSettingProp, new GUIContent("DSP Bus Setting"));
					}
					else
					{
						CriAtomAssetsPreviewPlayer.Instance.RegisterAcf(acfAsset);
						var num = CriAtomExAcf.GetNumDspSettings();
						var names = new List<string>() { "<none>" };
						for (ushort i = 0; i < num; i++)
							names.Add(CriAtomExAcf.GetDspSettingNameByIndex(i));

						var currentIndex = Mathf.Max(0, names.IndexOf(busSettingProp.stringValue));
						var newIndex = EditorGUILayout.Popup("DSP Bus Setting", currentIndex, names.ToArray());
						busSettingProp.stringValue = (newIndex == 0) ? null : names[newIndex];
					}

					EditorGUI.indentLevel++;
				}

				EditorGUILayout.Space();
				ReorderableList.DoLayoutList();
			}
			serializedObject.ApplyModifiedProperties();

			GUILayout.Label("Current Loaded CueSheets");
			EditorGUI.indentLevel++;
			using(new EditorGUI.DisabledScope(true))
			{
				foreach(var cuesheet in CriAtomAssetsLoader.Instance.CueSheets)
				{
					using(new GUILayout.HorizontalScope())
					{
						EditorGUILayout.ObjectField(cuesheet.AcbAsset, typeof(CriAtomAcbAsset), false);
						GUILayout.Label($"Ref Count:{cuesheet.ReferenceCount}");
					}
				}
			}
			EditorGUI.indentLevel--;
		}

		private void OnEnable()
		{
			EditorApplication.update += CheckForRepaint;
		}

		private void OnDisable()
		{
			EditorApplication.update -= CheckForRepaint;
		}

		int beforeCount = 0;
		void CheckForRepaint()
		{
			var count = CriAtomAssetsLoader.Instance.CueSheets.Count();
			if (count != beforeCount)
				Repaint();
			beforeCount = count;
		}
	}
}

/** @} */
