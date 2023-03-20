/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace CriWare.Assets
{
	/**
	 * <summary>The actual data location based on the asset deployment</summary>
	 */
	[Serializable]
	public class CriSerializedBytesAssetImpl : ICriMemoryAssetImpl, IDisposable
	{
		[SerializeField, HideInInspector]
		byte[] data;

		public CriSerializedBytesAssetImpl(byte[] data)
		{
			this.data = data;
		}

		GCHandle handle;

		public IntPtr PinnedAddress
		{
			get
			{
				if (!handle.IsAllocated)
					handle = GCHandle.Alloc(data, GCHandleType.Pinned);
				return handle.AddrOfPinnedObject();
			}
		}

		public int Size => data.Length;

		public byte[] Data => data;

		public bool IsReady => true;

		public void Dispose()
		{
			if (handle.IsAllocated)
				handle.Free();
		}

		public void OnEnable() { }
		public void OnDisable() => Dispose();
	}
}

/** @} */
