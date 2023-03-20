/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using UnityEngine;
using UnityEditor;

namespace CriWare.Assets {
    internal class CriManaUsmAssetPreviewPlayer : IPreviewPlayer {

        private float speed = 1.0f;
        PreviewRenderUtility renderUtility;
        CriManaMovieControllerForAsset movieController;
        CriManaUsmAsset usmAsset;

        public bool IsPlaying { get => movieController.player.status == CriMana.Player.Status.Playing; }
        public float Speed { get => speed; set => SetSpeed(value); }
        public float GetPlaybackTimeFromPlayer { get => GetDuration(); }

        private void InitializeLibrary() {
			CriManaAssetsPreviewer.Instance.InitializeLibrary();
		}

        private void SetSpeed(float speed){
            if (this.speed == speed || movieController.player.status != CriMana.Player.Status.Playing) {
                return;
            }
            this.speed = speed;
            var frameNo = movieController.player.GetDisplayedFrameNo();
            movieController.player.StopForSeek();
            while (true) {
                if (movieController.player.status == CriMana.Player.Status.Stop ||
                    movieController.player.status == CriMana.Player.Status.Error) {
                    break;
                }
                movieController.PlayerManualUpdate();
            }
            movieController.player.SetSeekPosition(frameNo);
            movieController.player.SetSpeed(speed);
            movieController.Play();
        }

        public void OnEnable() {
            this.InitializeLibrary();
            this.CreatePlayer();
        }

        public void OnDisable() {
            if (movieController == null) return;
            this.DestroyPlayer();
        }

        public void CreatePlayer() {
            renderUtility = new PreviewRenderUtility();
            var obj = new GameObject("previewer", typeof(UnityEngine.UI.Image), typeof(CriManaMovieControllerForAsset), typeof(CriWareErrorHandler));
            renderUtility.AddSingleGO(obj);
            movieController = obj.GetComponent<CriManaMovieControllerForAsset>();
            movieController.PlayerManualInitialize();
            movieController.PlayerManualSetup();
            movieController.RenderTargetManualSetup();
        }

        public void DestroyPlayer() {
            if (movieController == null) {
                return;
            }
            Stop();
            movieController.PlayerManualFinalize();
            movieController.RenderTargetManualFinalize();
            Object.DestroyImmediate(movieController.gameObject);
            movieController = null;
            renderUtility.Cleanup();
            renderUtility = null;
            usmAsset = null;
        }

        public float GetDuration(){
            if (usmAsset?.movieInfo == null || movieController.player.status != CriMana.Player.Status.Playing) {
                return 1;
            }
            var dispNo = movieController.player.GetDisplayedFrameNo();
            var d = dispNo % (float)usmAsset.movieInfo.totalFrames / (float)usmAsset.movieInfo.totalFrames;
            return d;
        }

        public void Pause() {
            movieController.Pause(!movieController.player.IsPaused());
        }

        public void Play() {
            movieController.Play();
        }

        public void Stop() {
            movieController.Stop();
            while (true)
            {
                if (movieController.player.status == CriMana.Player.Status.Stop ||
                    movieController.player.status == CriMana.Player.Status.Error) {
                    break;
                }
                movieController.PlayerManualUpdate();
            }
        }

        public bool UpdatePlayer() {
            movieController.PlayerManualUpdate();
            movieController.player.OnWillRenderObject(movieController);
            return movieController.player.HasRenderedNewFrame() || true; // for StopForSeek
        }

        public void SetAsset(CriAssetBase asset){
            usmAsset = (CriManaUsmAsset)asset;
            if (usmAsset is ICriReferenceAsset)
                usmAsset = (usmAsset as ICriReferenceAsset).ReferencedAsset as CriManaUsmAsset;
            movieController.player.SetFile(null, System.IO.Path.GetFullPath(AssetDatabase.GetAssetPath(usmAsset)));
            movieController.player.Loop(usmAsset.AssetInfo.loop);
            movieController.player.additiveMode = usmAsset.AssetInfo.additive;
        }

        public void Seek(float seekPosition) {
            movieController.player.StopForSeek();
            while (true) {
                if (movieController.player.status == CriMana.Player.Status.Stop ||
                    movieController.player.status == CriMana.Player.Status.Error) {
                    break;
                }
                movieController.PlayerManualUpdate();
            }

            movieController.player.SetSeekPosition(Mathf.RoundToInt(usmAsset.movieInfo.totalFrames * seekPosition));
            movieController.Play();
        }

        public Material GetMaterial(){
            return movieController.material;
        }

        public Rect ResizeRect(Rect baseRect) {
            if (usmAsset?.movieInfo == null) {
                return baseRect;
            }
            if(usmAsset.movieInfo.height == 0)
			{
                return baseRect;
			}

            float aspect = usmAsset.movieInfo.width / usmAsset.movieInfo.height;

            Rect resized = new Rect(baseRect);
            if (baseRect.width / baseRect.height >= aspect) {
                resized.width = baseRect.height * aspect;
                var space = (baseRect.width - resized.width) / 2.0f;
                resized.x += space;
            } else {
                resized.height = baseRect.width / aspect;
                var space = (baseRect.height - resized.height) / 2.0f;
                resized.y += space;
            }
            return resized;
        }
    }

}

/** @} */
