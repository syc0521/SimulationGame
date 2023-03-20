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
	/**
	 * <summary>Cue reference information structure</summary>
	 * <remarks>
	 * <para header='Description'>A structure that can be used to refer to a Cue by its ID and the ACB asset it belongs to.<br/>
	 * By serializing the fields of an CriAtomCueReference ,<br/>
	 * it is possible to choose a Cue from a drop-down list in the Editor.<br/></para>
	 * </remarks>
	 */
	[System.Serializable]
	public struct CriAtomCueReference
	{
		[SerializeField]
		CriAtomAcbAsset acbAsset;
		[SerializeField]
		int cueId;

		public CriAtomAcbAsset AcbAsset => acbAsset;
		public int CueId => cueId;

		public CriAtomCueReference(CriAtomAcbAsset acbAsset, int cueId)
		{
			this.acbAsset = acbAsset;
			this.cueId = cueId;
		}
	}
}

/** @} */
