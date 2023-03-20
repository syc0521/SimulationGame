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
using System.IO;
using System;

namespace CriWare.Assets
{
	/**
	 * <summary>USM Asset Class</summary>
	 * <remarks>
	 * <para header='Description'>This class handles USM files imported as Unity assets.<br/></para>
	 * </remarks>
	 */
	public class CriManaUsmAsset : CriAssetBase
	{
		[System.Serializable]
		public class ManaMovieInfo : CriMana.MovieInfo { }

		[System.Serializable]
		public class MovieAssetInfo
		{
			[SerializeField]
			public bool loop;
			[SerializeField]
			public bool additive;
			[SerializeField]
			public bool ambisonics;
		}

		[SerializeField]
		internal ManaMovieInfo movieInfo;
		[SerializeField]
		internal MovieAssetInfo assetInfo;

		/**
		 * <summary>Movie information within the USM file</summary>
		 * <remarks>
		 * <para header='Description'>Returns the movie information of the target USM. <br/>
		 * By using this property, it is possible to obtain information equivalent to the header without actually decoding and playing the movie.</para>
		 * </remarks>
		 */
		public virtual ManaMovieInfo MovieInfo {
			get => movieInfo;
		}

		/**
		 * <summary>Playback Parameters</summary>
		 * <remarks>
		 * <para header='Description'>Allows you to get/set the parameters used during playback in <see cref='CriMana.Player'/>.</para>
		 * </remarks>
		 */
		public virtual MovieAssetInfo AssetInfo {
			get => assetInfo;
		}
	}

	public static class CriManaPlayerExtentionForAsset
	{
		/**
		 * <summary>Playback Asset Specification</summary>
		 * <returns>Whether the set was successful</returns>
		 * <remarks>
		 * <para header='Description'>Set the assets to be played by this Player.<br/></para>
		 * </remarks>
		 */
		public static bool SetAsset(this CriMana.Player player, CriManaUsmAsset asset)
		{
			var result = false;

			var bytesImpl = asset.Implementation as ICriMemoryAssetImpl;
			if (bytesImpl != null)
				result = player.SetData(bytesImpl.PinnedAddress, bytesImpl.Size);

			var streamingImpl = asset.Implementation as ICriFileAssetImpl;
			if (streamingImpl != null)
				result = player.SetFileRange(streamingImpl.Path, streamingImpl.Offset, streamingImpl.Size);

			var cpkImpl = asset.Implementation as ICriFsAssetImpl;
			if (cpkImpl != null)
				result = player.SetContentId(cpkImpl.Binder, cpkImpl.ContentId);

			if (result)
			{
				player.Loop(asset.AssetInfo.loop);
				player.additiveMode = asset.AssetInfo.additive;
				player.atomExPlayer?.SetPanType(asset.assetInfo.ambisonics ? CriAtomEx.PanType.Pos3d : CriAtomEx.PanType.Pan3d);
				player.atomExPlayer?.UpdateAll();
				player.applyTargetAlpha = true;
			}

			return result;
		}
	}
}

/** @} */
