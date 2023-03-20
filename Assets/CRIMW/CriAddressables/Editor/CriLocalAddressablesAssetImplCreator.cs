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

using System.Security.Cryptography;
using System.IO;
using System;
using UnityEditor.AddressableAssets;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	[Serializable, CriDisplayName("Addressables (Local)")]
	public class CriLocalAddressablesAssetImplCreator : ICriAssetImplCreator
	{
		public string Description =>
@"This DeployType is deprecateed. Please use ""Addressables"" instead.
This deploy type should be used for assets included in Addressable Group built as a local bundle.
The actual data of the asset will be placed under LocalBuildPath when the bundle is built by the Addressables system.";

		public ICriAssetImpl CreateAssetImpl(AssetImportContext ctx)
		{
			if (AddressableAssetSettingsDefaultObject.Settings == null)
				throw new System.Exception($"[CRIWARE] AddressableAssetSettingsDefaultObject.Settings is null.\nCreate Addresasbles Settings and reimport the CRI Asset ({ctx.assetPath})");
			var md5 = MD5.Create();
			var hash = md5.ComputeHash(File.ReadAllBytes(ctx.assetPath));
			md5.Clear();
			var hashStr = BitConverter.ToString(hash).ToLower().Replace("-", "");
			
			return new CriLocalAddressablesAssetImpl(CriLocalAddressableGroupGenerator.GetAnchor(), Path.Combine(CriStreamingFolderAssetImplCreator.DirectoryName, $"{Path.GetFileName(ctx.assetPath)}{hashStr}"), new FileInfo(ctx.assetPath).Length, UnityEditor.AssetDatabase.AssetPathToGUID(ctx.assetPath));
		}
	}
}

#endif

/** @} */
