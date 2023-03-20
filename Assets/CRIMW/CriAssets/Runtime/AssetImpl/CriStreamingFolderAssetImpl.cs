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
using CriWare;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace CriWare.Assets
{
	/**
	 * <summary>The actual data location of the data in the StreamingAssets folder</summary>
	 */
	public class CriStreamingFolderAssetImpl : ICriFileAssetImpl
	{
		public CriStreamingFolderAssetImpl(string path, long size, string originalId)
		{
			_path = path;
			Size = size;
			_originalId = originalId;
		}

		[SerializeField]
		string _path;
		[SerializeField]
		string _originalId;

		public string Path
		{
#if UNITY_EDITOR
			get => System.IO.Path.GetFullPath(AssetDatabase.GUIDToAssetPath(_originalId));
#else
			get => System.IO.Path.Combine(CriWare.Common.streamingAssetsPath, _path);
#endif
		}

		internal string InternalPath => _path;

		public ulong Offset => 0;

		[field: SerializeField]
		public long Size { get; private set; }

		public bool IsReady => true;

		public void OnEnable() { }

		public void OnDisable() { }
	}
}

/** @} */
