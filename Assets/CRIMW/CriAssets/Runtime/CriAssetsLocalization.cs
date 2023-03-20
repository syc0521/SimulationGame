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
	 * <summary>CRI Assets multilingual control class</summary>
	 * <remarks>
	 * <para header='Description'>This class controls multilingual support for the CRI Assets.<br/>
	 * Only ACB assets have multilingual support at the moment.</para>
	 * </remarks>
	 */
	public class CriAssetsLocalization
	{
		/**
		 * <summary>Language currently selected</summary>
		 * <remarks>
		 * <para header='Description'>Returns the currently specified localization name. <br/>
		 * This is the string specified in <see cref='ChangeLanguage(string)'/>.</para>
		 * </remarks>
		 */
		public static string CurrentLanguage { get; private set; }

		/**
		 * <summary>Specify the localization language</summary>
		 * <remarks>
		 * <para header='Description'>Specify the target language of the localization.<br/>
		 * Please specify one of the localization language names set in the multilingual ACB assets.</para>
		 * </remarks>
		 */
		public static void ChangeLanguage(string name)
		{
			CurrentLanguage = name;
		}
	}
}

/** @} */
