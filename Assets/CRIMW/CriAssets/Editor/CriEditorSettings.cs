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
using System.Xml.Serialization;
using System.IO;

namespace CriWare.Assets
{
	public abstract class CriEditorSettingBase : ScriptableObject {
		UnityEditor.Editor _editor = null;
		internal UnityEditor.Editor Editor
		{
			get
			{
				if (_editor == null)
				{
					_editor = UnityEditor.Editor.CreateEditor(this);
				}
				return _editor;
			}
		}
	}

	public class CriEditorSettingsProvider : SettingsProvider
	{
		public CriEditorSettingsProvider(string path, SettingsScope scope)
			: base(path, scope) { }

		Dictionary<string, UnityEditor.Editor> editors = new Dictionary<string, UnityEditor.Editor>();

		public override void OnGUI(string searchContext)
		{
			var guids = AssetDatabase.FindAssets($"t:{nameof(CriEditorSettingBase)}");
			foreach(var guid in guids)
			{
				if (!editors.ContainsKey(guid))
					editors.Add(guid, UnityEditor.Editor.CreateEditor(AssetDatabase.LoadAssetAtPath<CriEditorSettingBase>(AssetDatabase.GUIDToAssetPath(guid))));
				EditorGUILayout.LabelField(editors[guid].target.name, EditorStyles.boldLabel);
				EditorGUI.indentLevel++;
				editors[guid].OnInspectorGUI();
				EditorGUI.indentLevel--;
			}
		}

		[SettingsProvider]
		static SettingsProvider Create()
		{
			var path = "Project/CRIWARE/Asset Support Add-on";
			var provider = new CriEditorSettingsProvider(path, SettingsScope.Project);
			return provider;
		}
	}
}

/** @} */
