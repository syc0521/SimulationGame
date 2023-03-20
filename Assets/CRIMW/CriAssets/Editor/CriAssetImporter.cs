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
using System.Runtime.CompilerServices;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

[assembly: InternalsVisibleTo("CriMw.CriWare.Assets.Addressables.Editor")]

namespace CriWare.Assets
{
	public abstract class CriAssetImporter : ScriptedImporter
	{
		[SerializeReference]
		public ICriAssetImplCreator implementation = new CriStreamingFolderAssetImplCreator();

		public abstract bool IsAssetImplCompatible { get; }

		protected ICriAssetImpl CreateAssetImpl(AssetImportContext ctx)
		{
			OnCreateImpl?.Invoke(ctx.assetPath);

			var ret = implementation.CreateAssetImpl(ctx);

			return ret;
		}

		public static System.Action<string> OnCreateImpl;
	}
}

/** @} */
