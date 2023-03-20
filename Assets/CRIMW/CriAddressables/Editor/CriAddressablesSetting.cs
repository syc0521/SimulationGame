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

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using System.Linq;

namespace CriWare.Assets
{
	public class CriAddressablesSetting : CriAssetSettingsBase<CriAddressablesSetting>
	{
		[SerializeField]
		public string anchorFolderPath = "CriData/Addressables";
		[SerializeField]
		public string deployFolderPath = "CriAddressables";
		[SerializeField]
		CriAddressablesPathPair remote;
		[SerializeField]
		CriAddressablesPathPair local;

		public string AnchorFolderPath => System.IO.Path.Combine("Assets", anchorFolderPath);

		static AddressableAssetSettings Settings => UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;

		internal CriAddressablesPathPair Remote { get
			{
				if (remote == null)
				{
					remote = CreateInstance<CriAddressablesPathPair>();
					remote.name = "Remote";
					remote.buildPath = new ProfileValueReference();
					remote.buildPath.SetVariableByName(Settings, Settings.profileSettings.GetVariableNames().Where(name => name.ToLowerInvariant().Contains("remote") && name.ToLowerInvariant().Contains("build")).FirstOrDefault());
					remote.loadPath = new ProfileValueReference();
					remote.loadPath.SetVariableByName(Settings, Settings.profileSettings.GetVariableNames().Where(name => name.ToLowerInvariant().Contains("remote") && name.ToLowerInvariant().Contains("load")).FirstOrDefault());
					AssetDatabase.AddObjectToAsset(remote, this);
					AssetDatabase.SaveAssets();
					AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
				}
				return remote;
			} }

		internal CriAddressablesPathPair Local
		{
			get
			{
				if (local == null)
				{
					local = CreateInstance<CriAddressablesPathPair>();
					local.name = "Local";
					local.buildPath = new ProfileValueReference();
					local.buildPath.SetVariableByName(Settings, Settings.profileSettings.GetVariableNames().Where(name => name.ToLowerInvariant().Contains("local") && name.ToLowerInvariant().Contains("build")).FirstOrDefault());
					local.loadPath = new ProfileValueReference();
					local.loadPath.SetVariableByName(Settings, Settings.profileSettings.GetVariableNames().Where(name => name.ToLowerInvariant().Contains("local") && name.ToLowerInvariant().Contains("load")).FirstOrDefault());
					AssetDatabase.AddObjectToAsset(local, this);
					AssetDatabase.SaveAssets();
					AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
				}
				return local;
			}
		}
	}

	[CustomEditor(typeof(CriAddressablesSetting))]
	public class CriAddressablesSettingEditor : UnityEditor.Editor
	{
		Dictionary<CriAddressablesPathPair, Editor> pathEditors = new Dictionary<CriAddressablesPathPair, Editor>();

		void DrawPathEditor(CriAddressablesPathPair pairObject) {
			if (!pathEditors.ContainsKey(pairObject))
				pathEditors.Add(pairObject, CreateEditor(pairObject));
			EditorGUILayout.LabelField(pairObject.name, EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			pathEditors[pairObject].OnInspectorGUI();
			EditorGUI.indentLevel--;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			DrawPathEditor((target as CriAddressablesSetting).Local);
			DrawPathEditor((target as CriAddressablesSetting).Remote);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("anchorFolderPath"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("deployFolderPath"), new GUIContent("Deploy Folder Suffix"));
			EditorGUILayout.LabelField($"{(target as CriAddressablesSetting).Remote.buildPath.GetValue(UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings)}/{"cridata"}_assets_{serializedObject.FindProperty("deployFolderPath").stringValue.ToLowerInvariant()}/", EditorStyles.boldLabel);
			serializedObject.ApplyModifiedProperties();
		}
	}
}

#endif

/** @} */
