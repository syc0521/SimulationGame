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
	class TargetAcfPair : TargetAssetPairGeneric<CriAtomAcfAsset> { }

	[ScriptedImporter(1, "multiacf", 3)]
	class CriAtomAcfMultiPlatformAssetImporter : CriMultiPlatformAssetImporter<CriAtomAcfAsset, CriAtomAcfMultiPlatformAsset, TargetAcfPair>
	{
		[MenuItem("Assets/Create/CRIWARE/MultiPlatform ACF")]
		static void CreateAsset() => CreateFile("multiacf");
	}
}

/** @} */
