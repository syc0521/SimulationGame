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
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif
using UnityEngine;

namespace CriWare.Assets
{
	[ScriptedImporter(2, "awb", 1)]
	class CriAtomAwbAssetImporter : CriAssetImporter
	{
		public override void OnImportAsset(AssetImportContext ctx)
		{
			var asset = ScriptableObject.CreateInstance<CriAtomAwbAsset>();
			asset.implementation = CreateAssetImpl(ctx);
			ctx.AddObjectToAsset("main", asset);
			ctx.SetMainObject(asset);
		}

		public override bool IsAssetImplCompatible => !(implementation is CriSerializedBytesAssetImplCreator);
	}
}

/** @} */
