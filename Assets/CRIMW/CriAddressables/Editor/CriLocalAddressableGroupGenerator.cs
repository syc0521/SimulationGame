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
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace CriWare.Assets
{
	internal static class CriLocalAddressableGroupGenerator
	{
		[InitializeOnLoadMethod]
		static void RegisterCallback() =>
			CriAddressablesEditor.OnAfterImportCriAsset += DestroyUnusedGroup;

		static CriAddressableGroup _group = null;
		static CriAddressableGroup Group => _group ?? (_group = new CriAddressableGroup("CriLocalData", "CriLocalPackedAssetsTemplate", CriAddressablesSetting.Instance.Local));

		public static AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;

		public static CriLocalAddressablesAnchor GetAnchor()
		{
			var anchor = AssetDatabase.LoadAssetAtPath<CriLocalAddressablesAnchor>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:{nameof(CriLocalAddressablesAnchor)}").FirstOrDefault()));

			if (Settings != null)
			{
				var path = AssetDatabase.GetAssetPath(anchor);
				var name = anchor.name;
				EditorApplication.delayCall += () =>
				{
					var entry = Settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), Group.GetOrCrate(), true);
					entry.address = $"{CriAddressablesSetting.Instance.deployFolderPath}/{name}";
				};
			}

			return anchor;
		}

		public static void DestroyUnusedGroup()
		{
			if (Settings == null) return;
			var criAddressableAssets = AssetDatabase.FindAssets($"t:{nameof(CriAssetBase)}").Select(guid => AssetDatabase.LoadAssetAtPath<CriAssetBase>(AssetDatabase.GUIDToAssetPath(guid))).Where(asset => !(asset is ICriReferenceAsset) && (asset.Implementation is CriLocalAddressablesAssetImpl));

			if (criAddressableAssets.Count() <= 0)
				Group.ClearGroup();
		}

		internal static void ValidateGroup()
		{
			if (Group.AddressableGroup == null) return;

			var schema = Group.AddressableGroup.GetSchema<UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema>();
			if (schema == null)
			{
				Debug.LogWarning("[CRIWARE] CriData Addressable Group does not have BundledAssetGroupSchema");
				return;
			}

			if (schema.AssetBundleProviderType.ToString() != nameof(CriLocalResourceProvider))
				Debug.LogWarning("[CRIWARE] CriLocalData Addressable Group have to use CriLocalResourceProvider as AssetBundleProvider");
			if (schema.BundleMode != UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundlePackingMode.PackTogether)
				Debug.LogWarning("[CRIWARE] CriLocalData Addressable Group have to use PackTogether BundleMode");
			if (schema.BundleNaming != UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema.BundleNamingStyle.NoHash)
				Debug.LogWarning("[CRIWARE] CriLocalData Addressable Group have to use NoHash BundleNaming");
		}
	}
}

#endif

/** @} */
