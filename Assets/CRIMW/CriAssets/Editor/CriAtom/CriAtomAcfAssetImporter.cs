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
using System.IO;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	[ScriptedImporter(2, "acf", 1)]
	class CriAtomAcfAssetImporter : CriAssetImporter
	{
		public override void OnImportAsset(AssetImportContext ctx)
		{
			var main = ScriptableObject.CreateInstance<CriAtomAcfAsset>();
			main.implementation = CreateAssetImpl(ctx);
			ctx.AddObjectToAsset("main", main);
			ctx.SetMainObject(main);
		}

		public override bool IsAssetImplCompatible => true;
	}
}

/** @} */
