/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(assemblyName: "CriMw.CriWare.Assets.Editor")]

namespace CriWare.Assets
{
	public static class CriAssets
	{
		private const string scriptVersionString = "0.4.01";
		private const int scriptVersionNumber = 0x00040100;
	}
}

/** @} */
