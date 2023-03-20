/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ADDRESSABLES_INTEGRATION
 * @{
 */

#if CRI_USE_ADDRESSABLES

using UnityEngine;

namespace CriWare.Assets
{
	public class CriAddressablesAnchor : ScriptableObject {
		[SerializeField, HideInInspector]
		internal string hash;
	}
}

#endif

/** @} */
