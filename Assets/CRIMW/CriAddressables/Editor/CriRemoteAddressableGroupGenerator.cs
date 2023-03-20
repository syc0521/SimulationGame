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
	internal static class CriRemoteAddressableGroupGenerator
	{
		static CriAddressableGroup _group = null;
		internal static CriAddressableGroup Group => _group ?? (_group = new CriAddressableGroup("CriData", "CriPackedAssetsTemplate", CriAddressablesSetting.Instance.Remote));

		public static (CriAddressablesAnchor anchor, string bundleName) CreateAnchor(string assetPath) =>
#if CRI_ADDRESSABLES_DISABLE_LEGACY_DEPLOY
			CriAddressableGroupGenerator.CreateAnchor(assetPath, Group, false);
#else
			CriAddressableGroupGenerator.CreateAnchor(assetPath, Group, true);
#endif

		internal static void ValidateGroup() => CriAddressableGroupGenerator.ValidateGroup(Group.AddressableGroup);

		public static void DeployData(string originalPath, AddressableAssetEntry entry)
		{
			var dataRootDir = CriAddressablesSetting.Instance.Remote.buildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings);
			var dataPath = Path.Combine(dataRootDir, $"{entry.parentGroup.Name.ToLowerInvariant()}_assets_{entry.address.ToLowerInvariant()}");

			CriAddressablesEditor.DeployData(originalPath, dataPath);

			// Delete bundles built by addressables
			var builtBundlePath = $"{dataPath}.bundle";
			if (File.Exists(builtBundlePath))
				File.Delete(builtBundlePath);
		}
	}
}

#endif

/** @} */
