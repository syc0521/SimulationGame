/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */



namespace CriWare.Assets {

    /**
     * <summary>Player interface for previewing</summary>
     * <remarks>
     * <para header='Description'>This is a player interface for previewing<br/>
	 * that can handle resources for playback in any format, including audio and video.<br/></para>
     *  <para header='Note'>The functions of this interface are expected to be called from a controlling class.<br/></para>
     * </remarks>
     */
    internal interface IPreviewPlayer {
        bool IsPlaying { get; }
        float Speed { get; set; }
        float GetPlaybackTimeFromPlayer { get; }

        void CreatePlayer();

        void OnEnable();

        void OnDisable();

        void DestroyPlayer();

        void Play();

        void Pause();

        void Stop();

        bool UpdatePlayer();

        void SetAsset(CriAssetBase asset);

        void Seek(float seekPosition);
    }
}

/** @} */
