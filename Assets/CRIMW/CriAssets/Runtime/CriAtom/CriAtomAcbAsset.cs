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
using System.Runtime.InteropServices;
using System;
using System.Linq;

namespace CriWare.Assets
{
	/**
	 * <summary>ACB Asset Class</summary>
	 * <remarks>
	 * <para header='Description'>A class that handles ACB files imported as Unity assets.<br/></para>
	 * </remarks>
	 */
	public class CriAtomAcbAsset : CriAssetBase
	{
		[SerializeField]
		internal CriAtomAwbAsset awb;

		CriAtomExAcb _handle = null;

		public virtual CriAtomAwbAsset Awb => awb;

		/**
		 * <summary>ACB instance</summary>
		 * <remarks>
		 * <para header='Description'>Gets an instance of the loaded ACB.<br/>
		 * Returns null if the loading is not complete.</para>
		 * </remarks>
		 */
		public CriAtomExAcb Handle { get {
				if (!CriAtomPlugin.IsLibraryInitialized()) return null;
				if(_handle == null)
				{
					if(Status == CriAtomExAcbLoader.Status.Complete)
					{
						_handle = asyncLoader.MoveAcb();
						asyncLoader.Dispose();
						asyncLoader = null;
					}
				}
				return _handle;
			} }

		/**
		 * <summary>Load completion callback</summary>
		 * <remarks>
		 * <para header='Description'>Called when the ACB file has been loaded by the library. <br/>
		 * All registered callbacks will be unregistered after a loading operation is completed.</para>
		 * </remarks>
		 */
		public event Action<CriAtomAcbAsset> OnLoaded = null;

		/**
		 * <summary>Whether there was a loading request</summary>
		 * <remarks>
		 * <para header='Description'>Whether loading is requested (by calling CriAtomAcbAsset.LoadAsync etc.).</para>
		 * </remarks>
		 */
		[field : System.NonSerialized]
		public bool LoadRequested { get; private set; } = false;

		/**
		 * <summary>Whether loading is completed</summary>
		 * <remarks>
		 * <para header='Description'>True when the loading initiated by calling CriAtomAcbAsset.LoadAsync has completed.</para>
		 * </remarks>
		 */
		public bool Loaded {
			get => Status == CriAtomExAcbLoader.Status.Complete;
		}

		/**
		 * <summary>Loading the Cue Sheet (asynchronously)</summary>
		 * <remarks>
		 * <para header='Description'>Loads the Cue Sheet asynchronously.<br/>
		 * When loading data with this method, the Cue Sheet will be available once the
		 * CriAtomAcbAsset.Loaded becomes true.<br/></para>
		 * </remarks>
		 */
		public void LoadAsync()
		{
			if (LoadRequested)
				throw new InvalidOperationException($"[CRIWARE] {name} ({nameof(CriAtomAcfAsset)}) is already loaded.");

			if (!InternalLoadAsync())
				throw new System.Exception("[CRIWARE] Load Acb Failed");
			_loadedAcbAssets.Add(new WeakReference<CriAtomAcbAsset>(this));
			LoadRequested = true;
			return;
		}

		/**
		 * <summary>Loading the Cue Sheet (synchronously)</summary>
		 * <remarks>
		 * <para header='Description'>Loads the Cue Sheet. <br/>
		 * This method is synchronous and may block the calling thread for a long time. <br/></para>
		 * </remarks>
		 */
		public void LoadImmediate()
		{
			if (LoadRequested)
				throw new InvalidOperationException($"[CRIWARE] {name} ({nameof(CriAtomAcfAsset)}) is already loaded.");

			if (!InternalLoadImmediate())
				throw new System.Exception("[CRIWARE] Load Acb Failed");
			_loadedAcbAssets.Add(new WeakReference<CriAtomAcbAsset>(this));
			LoadRequested = true;
			return;
		}

		/**
		 * <summary>Unloading the Cue Sheet</summary>
		 * <remarks>
		 * <para header='Description'>Unloading the Cue Sheet.</para>
		 * </remarks>
		 */
		public void Unload()
		{
			if (!LoadRequested) return;
			LoadRequested = false;
			InternalUnload();
			foreach (var reference in _loadedAcbAssets)
				if (reference.TryGetTarget(out CriAtomAcbAsset asset))
					if (asset == this)
					{
						_loadedAcbAssets.Remove(reference);
						break;
					}
		}

		bool InternalLoadAsync()
		{
			if (!CriAtomPlugin.IsLibraryInitialized()) return false;

			bool result = false;

#if UNITY_WEBGL && UNITY_EDITOR
			var loadAwbOnMemory = true;
#else
			var loadAwbOnMemory = false;
#endif

			if (!string.IsNullOrEmpty(FilePath))
			{
				asyncLoader = CriAtomExAcbLoader.LoadAcbFileAsync(null, FilePath, (Awb?.Implementation as ICriFileAssetImpl)?.Path, loadAwbOnMemory);
				result = true;
			}
			if (Data != null)
			{
				asyncLoader = CriAtomExAcbLoader.LoadAcbDataAsync(Data, null, (Awb?.Implementation as ICriFileAssetImpl)?.Path, loadAwbOnMemory);
				result = true;
			}

			if (result)
				CriAtomServer.instance.StartCoroutine(UpdateRoutine());

			return result;
		}

		bool InternalLoadImmediate()
		{
			if (!CriAtomPlugin.IsLibraryInitialized()) return false;

			bool result = false;

			if (!string.IsNullOrEmpty(FilePath))
			{
				_handle = CriAtomExAcb.LoadAcbFile(null, FilePath, (Awb?.Implementation as ICriFileAssetImpl)?.Path);
				result = true;
			}
			if (Data != null)
			{
				_handle = CriAtomExAcb.LoadAcbData(Data, null, (Awb?.Implementation as ICriFileAssetImpl)?.Path);
				result = true;
			}

			if (result)
				OnLoaded?.Invoke(this);
			OnLoaded = null;

			return result;
		}

		void InternalUnload()
		{
			LoadRequested = false;
			asyncLoader?.Dispose();
			asyncLoader = null;
			Handle?.Dispose();
			_handle = null;
		}

		IEnumerator UpdateRoutine()
		{
			while (Status != CriAtomExAcbLoader.Status.Complete)
				yield return null;
			OnLoaded?.Invoke(this);
			OnLoaded = null;
		}

		private void OnDisable()
		{
			Unload();
		}

		private void OnDestroy()
		{
			Unload();
		}

		~CriAtomAcbAsset()
		{
			Unload();
		}

		CriAtomExAcbLoader asyncLoader = null;

		/**
		 * <summary>Loading status of the asset</summary>
		 * <remarks>
		 * <para header='Description'>Returns the loading status of an asset.<br/>
		 * Check this status to determine if the asset requested by <see cref='LoadAsync'/> has been entirely loaded, etc.</para>
		 * </remarks>
		 */
		public CriAtomExAcbLoader.Status Status {
			get => asyncLoader?.GetStatus() ?? ((_handle == null) ? CriAtomExAcbLoader.Status.Stop : CriAtomExAcbLoader.Status.Complete);
		}

		static List<WeakReference<CriAtomAcbAsset>> _loadedAcbAssets = new List<WeakReference<CriAtomAcbAsset>>();
		internal static IEnumerable<CriAtomAcbAsset> LoadedAcbAssets =>
			_loadedAcbAssets.Select(reference => reference.TryGetTarget(out CriAtomAcbAsset target) ? target : null).Where(obj => obj != null);
	}
}

/** @} */
