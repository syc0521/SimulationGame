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
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	/**
	 * <summary>Actual data import processing interface</summary>
	 * <remarks>
	 * <para header='Description'>An interface that provides the definitions for importing the actual data of the CRIWARE assets.
	 * By inheriting this interface, you can add DeployType that can be set to CRIWARE assets.</para>
	 * </remarks>
	 */
	public interface ICriAssetImplCreator
	{
		/**
		 * <summary>Actual data import process</summary>
		 * <returns>Actual data storage destination information</returns>
		 * <remarks>
		 * <para header='Description'>A method that reads a CRIWARE file and returns the path where the actual data will be stored.
		 * Called when importing an asset.</para>
		 * </remarks>
		 */
		ICriAssetImpl CreateAssetImpl(AssetImportContext ctx);

		//void Cleanup(string assetPath);

		/**
		 * <summary>Description of DeployType</summary>
		 * <remarks>
		 * <para header='Description'>This is the description displayed in the Inspector for each DeployType.</para>
		 * </remarks>
		 */
		string Description { get; }
	}

	[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
	public class CriDisplayNameAttribute : System.Attribute
	{
		public CriDisplayNameAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}

/** @} */
