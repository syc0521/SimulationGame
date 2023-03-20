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
	internal class CriManaAssetsPreviewer : System.IDisposable
	{
		static CriManaAssetsPreviewer _instance = null;
		public static CriManaAssetsPreviewer Instance => _instance ?? (_instance = new CriManaAssetsPreviewer());

		public void InitializeLibrary()
		{
			if (!CriAtomPlugin.isInitialized)
				CriAtomPlugin.InitializeLibrary();
			if (!CriManaPlugin.isInitialized)
				CriWareInitializer.InitializeMana(new CriManaConfig());
		}

		~CriManaAssetsPreviewer()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (CriManaPlugin.IsLibraryInitialized())
				CriManaPlugin.FinalizeLibrary();
			if (CriAtomPlugin.IsLibraryInitialized())
				CriAtomPlugin.FinalizeLibrary();
			if (CriFsPlugin.IsLibraryInitialized())
				CriFsPlugin.FinalizeLibrary();
		}
	}
}

/** @} */
