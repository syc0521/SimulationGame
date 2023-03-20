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

namespace CriWare.Assets
{
	public class CriAtomAcbLocalizedAsset : CriAtomAcbAsset, ICriReferenceAsset
	{
		[System.Serializable]
		public struct LanguageAssetPair
		{
			public string language;
			public CriAtomAcbAsset asset;
		}

		[SerializeField]
		internal LanguageAssetPair[] assets;

		public override CriAtomAwbAsset Awb => (ReferencedAsset as CriAtomAcbAsset)?.Awb;
		public override ICriAssetImpl Implementation => (ReferencedAsset as CriAtomAcbAsset)?.Implementation;

		public CriAssetBase ReferencedAsset {
			get {
				if (assets == null) return null;
				if (assets.Length <= 0) return null;

				foreach (var pair in assets)
					if (pair.language == CriAssetsLocalization.CurrentLanguage)
						return pair.asset;
				foreach (var pair in assets)
					if ((CriAssetsLocalization.CurrentLanguage ?? string.Empty).Contains(pair.language))
						return pair.asset;
				return assets[0].asset;
			}
		}
	}
}

/** @} */
