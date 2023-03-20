/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_COMPONENT
 * @{
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CriWare.Assets
{
	/**
	 * <summary>Component for loading Atom assets</summary>
	 * <remarks>
	 * <para header='Description'>A component for loading / releasing an ACF / ACB asset.<br/>
	 * When the instance is created, the ACF and ACB specified in the inspector will be loaded.<br/>
	 * When the instance is destroyed, the ACB specified in the inspector will be released.<br/></para>
	 * </remarks>
	 */
	public class CriAtomAssets : MonoBehaviour
	{
		[SerializeField]
		CriAtomAcfAsset acfAsset = null;
		[SerializeField]
		string dspBusSetting = null;
		[SerializeField]
		CriAtomAcbAsset[] acbAssets = null;

		private void Awake()
		{
			CriAtomPlugin.InitializeLibrary();

			if (acfAsset != null)
			{
				if (!acfAsset.Register())
					throw new Exception("[CRIWARE] Register Acf Failed");
				if (!string.IsNullOrEmpty(dspBusSetting))
					CriAtomEx.AttachDspBusSetting(dspBusSetting);
			}
			foreach (var asset in acbAssets)
			{
				if (asset == null) continue;
				CriAtomAssetsLoader.AddCueSheet(asset);
			}
		}

		private void OnDestroy()
		{
			foreach (var asset in acbAssets)
			{
				if (asset == null) continue;
				CriAtomAssetsLoader.ReleaseCueSheet(asset, true);
			}

			CriAtomPlugin.FinalizeLibrary();
		}
	}

	/**
	 * <summary>Cue Sheet management class from the Asset Support add-on</summary>
	 * <remarks>
	 * <para header='Description'>This class manages the ACBs loaded either automatically by a CriAtomSourceForAsset or manually from CriAtomAssets.<br/>
	 * It keeps track of the reference count for each of the loaded ACBs.<br/></para>
	 * </remarks>
	 */
	public class CriAtomAssetsLoader
	{
		static CriAtomAssetsLoader _instance = null;
		public static CriAtomAssetsLoader Instance {
			get => _instance ?? (_instance = new CriAtomAssetsLoader());
		}

		/**
		 * <summary>Cue Sheet information class</summary>
		 * <remarks>
		 * <para header='Description'>A class that represents a loaded Cue Sheet from the list.<br/></para>
		 * </remarks>
		 */
		public class CueSheet
		{
			public CueSheet(string name, CriAtomAcbAsset asset)
			{
				Name = name;
				AcbAsset = asset;
				ReferenceCount = 0;
			}

			public string Name { get;private set; }
			public CriAtomAcbAsset AcbAsset { get; private set; }

			public int ReferenceCount { get; internal set; }
		}

		List<CueSheet> _cueSheets = new List<CueSheet>();

		/**
		 * <summary>List of loaded Cue Sheets</summary>
		 * <remarks>
		 * <para header='Description'>Returns the list of the loaded Cue Sheets.<br/></para>
		 * </remarks>
		 */
		public IEnumerable<CueSheet> CueSheets {
			get => _cueSheets;
		}

		/**
		 * <summary>Acquiring the Cue Sheet (by specifying the ACB asset)</summary>
		 * <remarks>
		 * <para header='Description'>Gets the information pertaining to the loaded Cue Sheet corresponding to the specified ACB asset.</para>
		 * </remarks>
		 */
		public CueSheet GetCueSheet(CriAtomAcbAsset asset)
		{
			foreach (var cueSheet in CueSheets)
				if (cueSheet.AcbAsset == asset)
					return cueSheet;
			return null;
		}

		/**
		 * <summary>Acquiring the Cue Sheet (by specifying the Cue Sheet's name)</summary>
		 * <remarks>
		 * <para header='Description'>Gets the information on the loaded Cue Sheet with the specified name.<br/>
		 * For assets registered without specifying a Cue Sheet name, the asset name will be used as the Cue Sheet name.<br/></para>
		 * </remarks>
		 */
		public CueSheet GetCueSheet(string name)
		{
			foreach (var cueSheet in CueSheets)
				if (cueSheet.Name == name)
					return cueSheet;
			return null;
		}

		/**
		 * <summary>Loading the Cue Sheet (by specifying the Cue Sheet's name)</summary>
		 * <remarks>
		 * <para header='Description'>Loads an ACB asset. If already loaded, the Cue Sheet's reference count will be increased.<br/>
		 * The Cue Sheet's information can be obtained by calling the GetCueSheet method with the name of the Cue Sheet of interest.<br/></para>
		 * </remarks>
		 */
		public void AddCueSheet(CriAtomAcbAsset acbAsset, string name)
		{
			var cueSheet = GetCueSheet(acbAsset);
			if (cueSheet == null)
			{
				cueSheet = new CueSheet(name, acbAsset);
				_cueSheets.Add(cueSheet);
				acbAsset.LoadAsync();
			}
			cueSheet.ReferenceCount++;
		}

		/**
		 * <summary>Loading the Cue Sheet</summary>
		 * <remarks>
		 * <para header='Description'>Loads an ACB asset. If already loaded, the Cue Sheet's reference count will be increased.<br/>
		 * The name of a Cue Sheet loaded with this method will be the same than the name of the asset.<br/></para>
		 * </remarks>
		 */
		public static void AddCueSheet(CriAtomAcbAsset acbAsset)
		{
			Instance.AddCueSheet(acbAsset, acbAsset.name);
		}

		/**
		 * <summary>Releasing the Cue Sheet (by specifying the ACB asset)</summary>
		 * <remarks>
		 * <para header='Description'>Releases an ACB asset. When the reference count reaches 0, the Cue Sheet will automatically be unloaded. <br/>
		 * However, if unloadImmediate is set to false, 
		 * UnloadUnusedCueSheets will need to be called later to unload it. <br/></para>
		 * </remarks>
		 */
		public static void ReleaseCueSheet(CriAtomAcbAsset acbAsset, bool unloadImmediate = true)
		{
			Instance.ReleaseCueSheet(Instance.GetCueSheet(acbAsset), unloadImmediate);
		}

		/**
		 * <summary>Releasing the Cue Sheet (by specifying the Cue Sheet's name)</summary>
		 * <remarks>
		 * <para header='Description'>Releases an ACB asset. When the reference count reaches 0, the Cue Sheet will automatically be unloaded. <br/>
		 * However, if unloadImmediate is set to false, 
		 * UnloadUnusedCueSheets will need to be called later to unload it. <br/></para>
		 * </remarks>
		 */
		public static void ReleaseCueSheet(string name, bool unloadImmediate = true)
		{
			Instance.ReleaseCueSheet(Instance.GetCueSheet(name), unloadImmediate);
		}

		/**
		 * <summary>Releasing the Cue Sheet</summary>
		 * <remarks>
		 * <para header='Description'>Releases an ACB asset. When the reference count reaches 0, the Cue Sheet will automatically be unloaded. <br/>
		 * However, if unloadImmediate is set to false, 
		 * UnloadUnusedCueSheets will need to be called later to unload it. <br/></para>
		 * </remarks>
		 */
		public void ReleaseCueSheet(CueSheet cueSheet, bool unloadImmediate)
		{
			if (cueSheet == null) return;
			cueSheet.ReferenceCount--;
			if (unloadImmediate)
				UnloadUnusedCueSheetsInternal();
		}

		internal void UnloadUnusedCueSheetsInternal()
		{
			for(int i = _cueSheets.Count - 1; i >= 0; i--)
			{
				if (_cueSheets[i].ReferenceCount <= 0)
				{
					_cueSheets[i].AcbAsset.Unload();
					_cueSheets.RemoveAt(i);
				}
			}
		}

		/**
		 * <summary>Unloading unreferenced Cue Sheets</summary>
		 * <remarks>
		 * <para header='Description'>Unloads all Cue Sheets with a reference count at 0 at once.<br/>
		 * If the unloading is delayed, call this function at the appropriate time.<br/></para>
		 * </remarks>
		 */
		public static void UnloadUnusedCueSheets() => Instance.UnloadUnusedCueSheetsInternal();
	}
}

/** @} */
