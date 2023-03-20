/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_COMPONENT
 * @{
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CriWare.Assets
{
	/**
	 * <summary>A component that plays audio from an asset</summary>
	 * <remarks>
	 * <para header='Description'>A component that can play audio by specifying an ACB asset and a Cue ID.<br/>
	 * It offers the same functionality than a CriAtomSource object while providing a different way to refer to a Cue.</para>
	 * </remarks>
	 */
	public class CriAtomSourceForAsset : CriAtomSourceBase
	{
		[SerializeField]
		CriAtomCueReference cue;

		public CriAtomCueReference Cue
		{
			get => cue;
			set => cue = value;
		}
		public override CriAtomExPlayback Play()
		{
			return Play(cue.CueId);
		}

		protected override CriAtomExAcb GetAcb()
		{
			return cue.AcbAsset.Handle;
		}

		bool ownLoadCount = false;
		protected override void PlayOnStart()
		{
			if (cue.AcbAsset == null) return;

			if(CriAtomAssetsLoader.Instance.GetCueSheet(cue.AcbAsset) == null && cue.AcbAsset.LoadRequested)
			{
				// loaded by user code
			}
			else
			{
				CriAtomAssetsLoader.AddCueSheet(cue.AcbAsset);
				ownLoadCount = true;
			}

			if (!playOnStart) return;

			if (cue.AcbAsset.Loaded)
			{
				player.SetCue(cue.AcbAsset.Handle, cue.CueId);
				InternalPlayCue();
			}
			else
			{
				cue.AcbAsset.OnLoaded += PlayCallback;
			}
		}

		void PlayCallback(CriAtomAcbAsset acbAsset)
		{
			player.SetCue(acbAsset.Handle, cue.CueId);
			InternalPlayCue();
		}

		protected override void InternalFinalize()
		{
			base.InternalFinalize();
			if (ownLoadCount)
				CriAtomAssetsLoader.ReleaseCueSheet(cue.AcbAsset);
			ownLoadCount = false;
		}
	}
}

/** @} */
