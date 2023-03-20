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
	 * <summary>ACF Asset Class</summary>
	 * <remarks>
	 * <para header='Description'>A class that handles ACF files imported as Unity assets.<br/></para>
	 * </remarks>
	 */
	public class CriAtomAcfAsset : CriAssetBase
	{
		/**
		 * <summary>ACF registration</summary>
		 * <remarks>
		 * <para header='Description'>Loads ACF into the library.</para>
		 * </remarks>
		 */
		public bool Register()
		{
			if (!CriAtomPlugin.IsLibraryInitialized())
				throw new System.Exception("[CRIWARE] Library not initialized");

			var fileImpl = Implementation as ICriFileAssetImpl;
			if (fileImpl != null)
			{
				CriAtomEx.RegisterAcf(null, fileImpl.Path);
				return true;
			}

			var memoryImpl = Implementation as ICriMemoryAssetImpl;
			if (memoryImpl != null)
			{
				CriAtomEx.RegisterAcf(memoryImpl.PinnedAddress, memoryImpl.Size);
				return true;
			}

			return false;
		}
	}
}

/** @} */
