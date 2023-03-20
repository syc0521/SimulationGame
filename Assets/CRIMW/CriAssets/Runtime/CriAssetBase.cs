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
	 * <summary>Base class of CRI asset</summary>
	 * <remarks>
	 * <para header='Description'>Base class for assets handled by the Asset Support add-on. <br/>
	 * This class contains the definitions (that are common to all assets) relevant to the handling of the actual data.<br/></para>
	 * </remarks>
	 */
	public abstract class CriAssetBase : ScriptableObject
	{
		[SerializeReference]
		internal ICriAssetImpl implementation;

		/**
		 * <summary>Actual data deployment destination information</summary>
		 * <remarks>
		 * <para header='Description'>A field that shows how the actual data is stored. <br/>
		 * The way to store the actual data depends on the inherited type.</para>
		 * </remarks>
		 */
		public virtual ICriAssetImpl Implementation {
			get => implementation;
		}

		/**
		 * <summary>Path to the data's raw file</summary>
		 * <remarks>
		 * <para header='Description'>Gets the path of the file containing the streaming playback data.<br/>
		 * Returns null if the actual data is stored in the asset.</para>
		 * </remarks>
		 */
		public string FilePath {
			get => (Implementation is ICriFileAssetImpl) ? (Implementation as ICriFileAssetImpl).Path : null;
		}

		/**
		 * <summary>Serialized data</summary>
		 * <remarks>
		 * <para header='Description'>Gets the actual data stored in the asset. <br/>
		 * Returns null if the actual data is in a non-asset file.</para>
		 * </remarks>
		 */
		public byte[] Data {
			get => (Implementation is ICriMemoryAssetImpl) ? (Implementation as ICriMemoryAssetImpl).Data : null;
		}

		void OnEnable() => Implementation?.OnEnable();
		void OnDisable() => Implementation?.OnDisable();
	}
}

/** @} */
