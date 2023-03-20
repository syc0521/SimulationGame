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

using UnityEngine.AddressableAssets;
using System.Linq;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ResourceManagement.Util;

[assembly:InternalsVisibleTo("CriMw.CriWare.Assets.Addressables.Editor")]

namespace CriWare.Assets
{
	/**
	 * <summary>Class that provides the functionality for the CRI Addressables</summary>
	 */
	public static class CriAddressables
	{
		private const string scriptVersionString = "0.5.05";
		private const int scriptVersionNumber = 0x00050500;

		/**
		 * <summary>Resource information update process</summary>
		 * <remarks>
		 * <para header='Description'>Updates the CRIWARE resource information read from the content catalog.<br/>
		 * Calls after initializing Addressables and after all content catalogs have been loaded.<br/>
		 * If you do not call this method, the download size of the content cannot be obtained correctly.<br/></para>
		 * </remarks>
		 */
		public static void ModifyLocators()
		{
			var original2criLocation = new Dictionary<IResourceLocation, IResourceLocation>();

			var locations =
					Addressables.ResourceLocators.
					Where(locator => locator is ResourceLocationMap).
					SelectMany(map => (map as ResourceLocationMap).Locations);
			foreach (var list in locations)
			{
				for (int i = 0; i < list.Value.Count; i++)
				{
					if (list.Value[i].ProviderId != typeof(CriResourceProvider).FullName) continue;
					if (list.Value[i] is CriResourceLocation) continue;

					if (!original2criLocation.ContainsKey(list.Value[i]))
						original2criLocation.Add(list.Value[i], new CriResourceLocation(list.Value[i]));

					list.Value[i] = original2criLocation[list.Value[i]];
				}
			}
		}

		internal static string LocalLoadPath { get; set; }

		/**
		 * <summary>Clearing asset cache</summary>
		 * <remarks>
		 * <para header='Description'>Removes the cache of the actual data downloaded via Addressables.<br/>
		 * Nothing will be executed if the asset DeployType is not Addressables.</para>
		 * </remarks>
		 */
		public static void ClearAddressableCache(this CriAssetBase asset)
		{
			(asset.Implementation as CriAddressableAssetImpl)?.ClearCache();
		}

		static Dictionary<string, string> _filename2Path { get; } = new Dictionary<string, string>();
		static Dictionary<string, long> _filename2Size { get; } = new Dictionary<string, long>();
		internal static void AddCachePath(string fileName, string path, long size)
		{
			if (_filename2Path.ContainsKey(fileName)) return;
			_filename2Path.Add(fileName, path);
			_filename2Size.Add(fileName, size);
		}
		internal static string Filename2CachePath(string fileName) => _filename2Path.ContainsKey(fileName) ? _filename2Path[fileName] : null;
		internal static long Filename2CahceSize(string fileName) => _filename2Size.ContainsKey(fileName) ? _filename2Size[fileName] : 0;

		internal static (string remote, string local) ResourceLocation2Path(IResourceLocation location)
		{
			var internalId =
#if ADDRESSABLES_1_13_1_OR_NEWER
				Addressables.ResourceManager.TransformInternalId(location);
#else
				location.InternalId;
#endif
			var remotePath = internalId;

			if (!ResourceManagerConfig.ShouldPathUseWebRequest(internalId) || (Application.platform == RuntimePlatform.Android && internalId.StartsWith("jar:")))
				return (remotePath, remotePath);

#if ENABLE_CACHING
			var cacheRoot = Caching.currentCacheForWriting.path;
#else
/* @cond notpublic */
#if UNITY_SWITCH
			CriWareSwitch.SetupTemporaryStorage();
#endif // UNITY_SWITCH
/* @endcond */
			var cacheRoot = CriWare.Common.installCachePath;
/* @cond notpublic */
#if !UNITY_EDITOR && UNITY_SWITCH
			cacheRoot = cacheRoot.Replace($"{CriWareSwitch.TemporaryStorageName}:/", $"/{CriWareSwitch.TemporaryStorageName}/");
#endif // !UNITY_EDITOR && UNITY_SWITCH
/* @endcond */
#endif // ENABLE_CACHING

			if (!Directory.Exists(cacheRoot))
				Directory.CreateDirectory(cacheRoot);

			var requestOptions = location.Data as AssetBundleRequestOptions;

			var localPath = Path.Combine(cacheRoot, requestOptions.BundleName, requestOptions.Hash, Path.GetFileName(location.InternalId));

			return (remotePath, localPath);
		}
	}
}

#endif

/** @} */
