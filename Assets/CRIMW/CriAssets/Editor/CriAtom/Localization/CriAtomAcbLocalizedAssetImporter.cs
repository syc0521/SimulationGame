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
using System.Linq;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	[ScriptedImporter(0, "localizedacb", 3)]
	class CriAtomAcbLocalizedAssetImporter : ScriptedImporter
	{
		[System.Serializable]
		public struct LanguageAssetsPair
		{
			public string language;
			public List<CriAtomAcbAsset> assets;
		}

		[SerializeField]
		List<BuildTarget> platforms = new List<BuildTarget>();
		[SerializeField]
		List<LanguageAssetsPair> assetsTable = new List<LanguageAssetsPair>() {
			new LanguageAssetsPair(){ language = "ja", assets = new List<CriAtomAcbAsset>() { null } }
		};

		[MenuItem("Assets/Create/CRIWARE/Localized ACB")]
		static void CreateAsset() {
			ProjectWindowUtil.CreateAssetWithContent($"New Licalized ACB Asset.localizedacb", "");
			AssetDatabase.Refresh();
		}

		public override void OnImportAsset(AssetImportContext ctx)
		{
			var main = ScriptableObject.CreateInstance<CriAtomAcbLocalizedAsset>();
			main.assets = assetsTable.Select(pair =>
				new CriAtomAcbLocalizedAsset.LanguageAssetPair()
				{
					language = pair.language,
					asset = pair.assets[platforms.IndexOf(ctx.selectedBuildTarget) + 1],
				}
			).ToArray();

			ctx.AddObjectToAsset("main", main);
			ctx.SetMainObject(main);
		}
	}
}

/** @} */
