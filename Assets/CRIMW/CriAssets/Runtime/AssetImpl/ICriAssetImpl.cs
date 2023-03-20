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
	 * <summary>Data storage destination interface</summary>
	 * <remarks>
	 * <para header='Description'>This is the common interface of the classes that represent the storage location of the actual data held by an asset.<br/>
	 * <see cref='CriAssetBase'/> can have a data storage location by implementing the methods of this interface.</para>
	 * </remarks>
	 */
	public interface ICriAssetImpl {

		/**
		 * <summary>Whether the data is available</summary>
		 */
		bool IsReady { get; }

		/**
		 * <summary>Processing when an asset is enabled</summary>
		 * <remarks>
		 * <para header='Description'>Called when the corresponding CRI asset becomes Enabled. <br/>
		 * The data reference setup process etc. will be performed.</para>
		 * </remarks>
		 */
		void OnEnable();

		/**
		 * <summary>Processing when an asset is disabled</summary>
		 * <remarks>
		 * <para header='Description'>Called when the corresponding CRI asset becomes Disable.<br/>
		 * The resources allocated in the instance will be destroyed.</para>
		 * </remarks>
		 */
		void OnDisable();
	}

	/**
	 * <summary>Actual data storage destination interface (in-memory)</summary>
	 * <remarks>
	 * <para header='Description'>Provides an interface to get information about in-memory data.<br/>
	 * You can customize the storage destination of the CRIWARE assets by inheriting this interface.</para>
	 * <seealso cref='CriSerializedBytesAssetImpl'/>
	 * </remarks>
	 */
	public interface ICriMemoryAssetImpl : ICriAssetImpl
	{
		/**
		 * <summary>Pointer to in-memory data</summary>
		 * <remarks>
		 * <para header='Description'>Returns a pointer to the start of the data fixed in memory. <br/>
		 * Can be used to pass data via pointer to the native wrapper API.</para>
		 * </remarks>
		 */
		System.IntPtr PinnedAddress { get; }
		/**
		 * <summary>Data size</summary>
		 * <remarks>
		 * <para header='Description'>Data size in memory.<br/>
		 * Use this when passing a pointer via <see cref='PinnedAddress'/>.</para>
		 * </remarks>
		 */
		System.Int32 Size { get; }
		/**
		 * <summary>In-memory data</summary>
		 * <remarks>
		 * <para header='Description'>Data expressed as a byte array.</para>
		 * </remarks>
		 */
		System.Byte[] Data { get; }
	}

	/**
	 * <summary>Actual data storage destination interface (file)</summary>
	 * <remarks>
	 * <para header='Description'>Provides an interface to get information about data that exists as a file.<br/>
	 * You can customize the storage destination of the CRIWARE assets by inheriting this interface.</para>
	 * <seealso cref='CriStreamingFolderAssetImpl'/>
	 * </remarks>
	 */
	public interface ICriFileAssetImpl : ICriAssetImpl
	{
		/**
		 * <summary>Non-Asset CRI Data Path</summary>
		 * <remarks>
		 * <para header='Description'>Path to the Non-Asset CRI data which holds the actual data.<br/></para>
		 * </remarks>
		 */
		string Path { get; }

		/**
		 * <summary>Data position in the file</summary>
		 * <remarks>The offset of the target data within the file specified by <see cref='Path'/>.<br/></remarks>
		 */
		ulong Offset { get; }

		/**
		 * <summary>Data Size</summary>
		 * <remarks>
		 * <para header='Description'>The size of the data in the file.</para>
		 * </remarks>
		 */
		long Size { get; }
	}

	public interface ICriFsAssetImpl : ICriAssetImpl
	{
		CriFsBinder Binder { get; }
		int ContentId { get; }
	}
}

/** @} */
