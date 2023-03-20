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
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	[System.Serializable]
	class TargetUsmPair : TargetAssetPairGeneric<CriManaUsmAsset> { }

	[ScriptedImporter(1, "multiusm", 3)]
	class CriManaUsmMultiPlatformAssetImporter : CriMultiPlatformAssetImporter<CriManaUsmAsset, CriManaUsmMultiPlatformAsset, TargetUsmPair>
	{
		[MenuItem("Assets/Create/CRIWARE/MultiPlatform USM")]
		static void CreateAsset() => CreateFile("multiusm");
	}
}

/** @} */
