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
	public class CriManaUsmMultiPlatformAsset : CriManaUsmAsset, ICriReferenceAsset
	{
		[SerializeField]
		CriManaUsmAsset original = null;

		public override MovieAssetInfo AssetInfo => original?.AssetInfo;
		public override ManaMovieInfo MovieInfo => original?.MovieInfo;

		public override ICriAssetImpl Implementation  => original?.Implementation;

		public CriAssetBase ReferencedAsset  => original;
	}
}

/** @} */
