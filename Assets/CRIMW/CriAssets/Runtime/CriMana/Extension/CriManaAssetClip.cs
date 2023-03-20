/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */


#if CRIWARE_TIMELINE_1_OR_NEWER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriWare.Assets
{
	public class CriManaAssetClip : CriTimeline.Mana.CriManaClipBase
	{
		[SerializeField]
		CriManaUsmAsset usmAsset = null;

		public CriManaUsmAsset UsmAsset
		{
			get => usmAsset;
			set => usmAsset = value;
		}

		public override string MoviePath => usmAsset?.FilePath;

		public override byte[] MovieData => usmAsset?.Data;

		public override string MovieName => usmAsset?.name;

		public override int DataId => usmAsset?.GetInstanceID() ?? 0;
	}
}

#endif

/** @} */
