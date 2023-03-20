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
	public class CriAtomAcbMultiPlatformAsset : CriAtomAcbAsset, ICriReferenceAsset
	{
		[SerializeField]
		CriAtomAcbAsset original = null;

		public override CriAtomAwbAsset Awb => original?.Awb;
		public override ICriAssetImpl Implementation => original?.Implementation;

		public CriAssetBase ReferencedAsset => original;
	}
}

/** @} */
