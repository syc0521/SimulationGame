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
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif
using System.IO;
using UnityEditor;

namespace CriWare.Assets
{
	[ScriptedImporter(1, "usm", 1)]
	class CriManaUsmAssetImporter : CriAssetImporter
	{
		[System.Serializable]
		class UsmImporterInfo : CriManaUsmAsset.MovieAssetInfo
		{
			[SerializeField]
			public bool importMovieInfo = true;
		}

		[SerializeField]
		UsmImporterInfo assetInfo = null;

		public override void OnImportAsset(AssetImportContext ctx)
		{
			var main = ScriptableObject.CreateInstance<CriManaUsmAsset>();
			main.assetInfo = assetInfo;
			main.implementation = CreateAssetImpl(ctx);

			if (assetInfo.importMovieInfo)
			{
				var movieInfo = GetMovieInfo(ctx.assetPath);
				var info = JsonUtility.ToJson(movieInfo);
				main.movieInfo = JsonUtility.FromJson<CriManaUsmAsset.ManaMovieInfo>(info);
			}

			ctx.AddObjectToAsset("main", main);
			ctx.SetMainObject(main);
		}

		CriMana.MovieInfo GetMovieInfo(string path)
		{
			CriManaAssetsPreviewer.Instance.InitializeLibrary();
			using (var player = new CriMana.Player())
			{
				player.SetFile(null, Path.GetFullPath(path));
				player.Prepare();
				for(int i = 0; ; i++)
				{
					if (player.movieInfo != null)
						return player.movieInfo;
					if (player.status == CriMana.Player.Status.Error || i > 10000)
					{
						Debug.LogWarning($"[CRIWARE] Mana Usm Asset Importer ({path}) failed to get movie info.");
						return null;
					}
					player.Update();
				}
			}
		}

		public override bool IsAssetImplCompatible => true;
	}
}

/** @} */
