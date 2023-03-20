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
using System;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace CriWare.Assets
{
	internal static class CriAddressableGroupGenerator
	{
		internal static event System.Action onAssetImported;

		class PostProcessFook : AssetPostprocessor
		{
			static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
			{
				onAssetImported?.Invoke();
				onAssetImported = null;
			}
		}

		[InitializeOnLoadMethod]
		static void RegisterCallback() =>
			CriAddressablesEditor.OnAfterImportCriAsset += DestroyUnusedAnchors;

		static string CriDataPath
		{
			get
			{
				if (!Directory.Exists(CriAddressablesSetting.Instance.AnchorFolderPath))
					Directory.CreateDirectory(CriAddressablesSetting.Instance.AnchorFolderPath);
				return CriAddressablesSetting.Instance.AnchorFolderPath;
			}
		}

		public static AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;

		public static (CriAddressablesAnchor anchor, string bundleName) CreateAnchor(string assetPath, CriAddressableGroup group, bool appendSize = false)
		{
			var anchorPath = appendSize ?
				Path.Combine(CriDataPath, $"{Path.GetFileName(assetPath).ToLowerInvariant()}={new FileInfo(assetPath).Length.ToString("x")}.asset") :
				Path.Combine(CriDataPath, $"{Path.GetFileName(assetPath).ToLowerInvariant()}.asset");

			var anchor = AssetDatabase.LoadAssetAtPath<CriAddressablesAnchor>(anchorPath);
			if (anchor == null)
			{
				anchor = ScriptableObject.CreateInstance<CriAddressablesAnchor>();
				var md5 = MD5.Create();
				var hash = md5.ComputeHash(File.ReadAllBytes(assetPath));
				md5.Clear();
				anchor.hash = BitConverter.ToString(hash).ToLower().Replace("-", "");
				AssetDatabase.CreateAsset(anchor, anchorPath);
				AssetDatabase.SaveAssets();
			}

			if (Settings == null)
				throw new Exception("[CRIWARE] AddresablesSettings must be ready when loading CRI Addressable asset.");

			var path = AssetDatabase.GetAssetPath(anchor);
			var name = anchor.name;
			string groupId;
			string address = $"{CriAddressablesSetting.Instance.deployFolderPath}/{name}";
			if (group.AddressableGroup == null)
			{
				// wee need guid before create group.
				groupId = GUID.Generate().ToString();
				// we can not create group while importing the asset.
				onAssetImported += () => {
					var entry = Settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group.GetOrCrate(), true);
					entry.address = address;
					// HACK : ineject pre-generated id.
					var serializedGroup = new SerializedObject(entry.parentGroup);
					serializedGroup.FindProperty("m_GUID").stringValue = groupId;
					serializedGroup.ApplyModifiedPropertiesWithoutUndo();
				};
			}
			else
			{
				var entry = Settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group.AddressableGroup, true);
				entry.address = address;
				groupId = entry.parentGroup.Guid;
			}
			var bundleName = CriAddressablesEditor.CalclateBundleName(groupId, address);
			return (anchor, bundleName);
		}

		public static void DestoroyAnchor(CriAddressablesAnchor anchor)
		{
			if (Settings?.profileSettings == null) return;

			var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(anchor));
			var group = Settings.FindAssetEntry(guid)?.parentGroup;

			if (group != null)
			{
				// Delete non-asset data if exist
				var dataRootDir = CriAddressablesSetting.Instance.Remote.buildPath.GetValue(Settings);
				var dataPath = Path.Combine(dataRootDir, $"{group.Name.ToLowerInvariant()}_assets_{Settings.FindAssetEntry(guid)?.address?.ToLowerInvariant()}");
				if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
					Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
				if (File.Exists(dataPath))
					File.Delete(dataPath);

				Settings.RemoveAssetEntry(guid);
			}

			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(anchor));
			AssetDatabase.SaveAssets();
		}

		public static void DestroyUnusedAnchors()
		{
			var criAddressableAssets = AssetDatabase.FindAssets($"t:{nameof(CriAssetBase)}").Select(guid => AssetDatabase.LoadAssetAtPath<CriAssetBase>(AssetDatabase.GUIDToAssetPath(guid))).Where(asset => !(asset is ICriReferenceAsset) && (asset.Implementation is CriAddressableAssetImpl)).ToList();
			var anchors = AssetDatabase.FindAssets($"t:{nameof(CriAddressablesAnchor)}").Select(guid => AssetDatabase.LoadAssetAtPath<CriAddressablesAnchor>(AssetDatabase.GUIDToAssetPath(guid))).ToList();

			if (criAddressableAssets.Count() < anchors.Count) {
				foreach (var asset in criAddressableAssets)
					anchors.Remove((asset.Implementation as CriAddressableAssetImpl).anchor);

				foreach (var anchor in anchors)
				{
					if (string.IsNullOrEmpty(anchor.hash)) continue;
					DestoroyAnchor(anchor);
				}
			}

			// can not delete group while importing asset
			EditorApplication.delayCall += DestroyUnusedGroups;
		}

		static void DestroyUnusedGroups() {
			var groups = Settings.groups
					.Where(g => g.entries.Count == 0)
					.Where(g => g.Name.Contains("CriData"))
					.Where(g => {
						var schema = g.GetSchema<UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema>();
						if (schema == null) return false;
						return schema.AssetBundleProviderType.Value == typeof(CriResourceProvider);
					})
					.ToList();
			foreach (var group in groups)
				Settings.RemoveGroup(group);
			AssetDatabase.SaveAssets();
		}

		internal static void ValidateGroup(AddressableAssetGroup group)
		{
			if (group == null) return;

			var schema = group.GetSchema<UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema>();
			if (schema == null)
			{
				Debug.LogWarning("[CRIWARE] CriData Addressable Group does not have BundledAssetGroupSchema");
				return;
			}

			if(schema.InternalBundleIdMode != UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundleInternalIdMode.GroupGuid)
				Debug.LogWarning("[CRIWARE] CriData Addressable Group have to use [Group Guid] as [Internal Bundle Id Mode]");
			if (schema.AssetBundleProviderType.ToString() != nameof(CriResourceProvider))
				Debug.LogWarning("[CRIWARE] CriData Addressable Group have to use CriResourceProvider as AssetBundleProvider");
			if (schema.BundleMode != UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundlePackingMode.PackSeparately)
				Debug.LogWarning("[CRIWARE] CriData Addressable Group have to use PackSeparately BundleMode");
			if (schema.BundleNaming != UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundleNamingStyle.NoHash)
				Debug.LogWarning("[CRIWARE] CriData Addressable Group have to use NoHash BundleNaming");
		}
	}
}

#endif

/** @} */
