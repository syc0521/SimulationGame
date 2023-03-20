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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;
using System.Linq;
using UnityEngine.ResourceManagement;
using System.IO;
using UnityEngine.ResourceManagement.Util;

namespace CriWare.Assets
{
	/**
	 * <summary>Data size implementation class for CRI Assets</summary>
	 */
	[System.Serializable]
	public class CriLocationSizeData : AssetBundleRequestOptions
	{
		internal static bool TryParseSize(string internalId, out long size)
		{
			try
			{
				size = Convert.ToInt64(internalId.Split('=').Last(), 16);
				return true;
			}
			catch
			{
				size = 0;
				return false;
			}
		}

		internal long GetResourceSize(IResourceLocation location)
		{
			if (TryParseSize(location.InternalId, out long size))
				return size;
			else
				return BundleSize;
		}

		public override long ComputeSize(IResourceLocation location, ResourceManager resourceManager)
		{
			var pathSet = CriAddressables.ResourceLocation2Path(location);
			if (!ResourceManagerConfig.IsPathRemote(pathSet.remote))
				return 0;
#if ENABLE_CACHING
			if (File.Exists(pathSet.local))
				return 0;
#endif
			return GetResourceSize(location);
		}
	}

	/**
	 * <summary>Class representing the Addressables location information of the CRI Asset</summary>
	 */
	public class CriResourceLocation : IResourceLocation
	{
        int m_HashCode;

        public string InternalId { get; }
		public string ProviderId => typeof(CriResourceProvider).FullName;
        public IList<IResourceLocation> Dependencies => null;
        public bool HasDependencies => false;
        public int DependencyHashCode => 0;
        public object Data { get; }

        public string PrimaryKey { get; }

        public Type ResourceType => typeof(IAssetBundleResource);

        public override string ToString() => InternalId;

        public int Hash(Type t) => (m_HashCode * 31 + t.GetHashCode()) * 31 + DependencyHashCode;

        public CriResourceLocation(IResourceLocation original)
        {
#if CRI_ADDRESSABLES_DISABLE_LEGACY_DEPLOY
			InternalId = original.InternalId;
#else
			InternalId = CriLocationSizeData.TryParseSize(Path.ChangeExtension(original.InternalId, null), out long _) ?
				Path.ChangeExtension(original.InternalId, null) :
				original.InternalId;
#endif
			m_HashCode = original.InternalId.GetHashCode() * 31 + ProviderId.GetHashCode();
			PrimaryKey = original.PrimaryKey;
			if (original.Data is AssetBundleRequestOptions)
				Data = JsonUtility.FromJson<CriLocationSizeData>(JsonUtility.ToJson(original.Data));
        }
    }
}

#endif

/** @} */
