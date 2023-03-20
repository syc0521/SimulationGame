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
	class TargetAcbPair : TargetAssetPairGeneric<CriAtomAcbAsset> { }

	[ScriptedImporter(1, "multiacb", 3)]
	class CriAtomAcbMultiPlatformAssetImporter : CriMultiPlatformAssetImporter<CriAtomAcbAsset, CriAtomAcbMultiPlatformAsset, TargetAcbPair>
	{
		[MenuItem("Assets/Create/CRIWARE/MultiPlatform ACB")]
		static void CreateAsset() => CreateFile("multiacb");
	}
}

/** @} */
