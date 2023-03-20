/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using UnityEditor;
using UnityEngine;

namespace CriWare.Assets {

    /**
     * <summary>Controller for the preview window</summary>
     * <remarks>
     * <para header='Description'>Controller class to operate the IPreviewPlayer interface.<br/>
	 * It should be called from an Editor extension class with HasPreviewGUI set to true.<br/></para>
     * </remarks>
     */
    internal class PreviewWindowController {
        internal IPreviewPlayer playbackController;

        private Color playheadColor = EditorGUIUtility.isProSkin ?
            new Color(239 / 255f, 239 / 255f, 244 / 255f) // Light
            : new Color(32 / 255f, 47 / 255f, 97 / 255f); // Dark
        private float previewWindowFps = 0.0f;
        private double lastFrameEditorTime;
        private int fpsCount = 0;

        internal PreviewWindowController(IPreviewPlayer _playbackController) {
            playbackController = _playbackController;
            EditorApplication.update += Update;
        }

        ~PreviewWindowController() {
            playbackController.OnDisable();
        }

        internal void OnEnable(){
            playbackController.OnEnable();
        }

        internal void OnDisable() {
            EditorApplication.update -= Update;

            playbackController.OnDisable();
        }

        internal void Update() {
            fpsCount++;
            var timeSinceStartup = EditorApplication.timeSinceStartup;
            var deltaTime = timeSinceStartup - lastFrameEditorTime;
            if (deltaTime > 0.5) {
                previewWindowFps = (float)(fpsCount / deltaTime);
                fpsCount = 0;
                lastFrameEditorTime = timeSinceStartup;
            }
        }

        internal void OnPreviewSettings(bool isSupportedChangeSpeed) {
            DrawPlayButton();
            if (isSupportedChangeSpeed) {
                DrawSpeedSlider();
            }
        }

        private void DrawPlayButton() {
            var playButtonContent = EditorGUIUtility.IconContent("PlayButton");
            var pauseButtonContent = EditorGUIUtility.IconContent("PauseButton");
            var previewButtonSettingsStyle = new GUIStyle("preButton");
            var buttonContent = playbackController.IsPlaying ? pauseButtonContent : playButtonContent;

            EditorGUI.BeginChangeCheck();

            var isPlaying = GUILayout.Toggle(playbackController.IsPlaying, buttonContent, previewButtonSettingsStyle);

            if (EditorGUI.EndChangeCheck()) {
                if (isPlaying) {
                    playbackController.Play();
                } else {
                    playbackController.Pause();
                }
            }
        }

        private void DrawSpeedSlider() {
            var preSlider = new GUIStyle("preSlider");
            var preSliderThumb = new GUIStyle("preSliderThumb");
            var preLabel = new GUIStyle("preLabel");
            var speedScale = EditorGUIUtility.IconContent("SpeedScale");

            GUILayout.Box(speedScale, preLabel);
            playbackController.Speed = GUILayout.HorizontalSlider(playbackController.Speed, 0.1f, 2f, preSlider, preSliderThumb);
            playbackController.Speed = Mathf.Floor(playbackController.Speed * 10f) / 10f;
            GUILayout.Label(playbackController.Speed.ToString("0.0"), preLabel, GUILayout.Width(30));
        }

        internal Rect DrawPlaybackSequencesSlider(Rect rect) {
            Rect timeline = rect;
            timeline.height = 21f;
            GUI.Box(timeline, GUIContent.none, "TimeScrubber");

            Rect scrubber = timeline;
            float currentPosition = Mathf.Lerp(scrubber.x, scrubber.xMax, playbackController.GetPlaybackTimeFromPlayer);
            float thickness = 3f;
            float halfThickness = thickness * 0.5f;
            Rect labelRect = Rect.MinMaxRect(
                currentPosition - halfThickness,
                scrubber.yMin,
                currentPosition + halfThickness,
                scrubber.yMax);
            EditorGUI.DrawRect(labelRect, playheadColor); // Draw Playhead
            return scrubber;
        }

        internal bool UpdatePreviewWindow() {
            bool isNeedRepaint = false;

            isNeedRepaint = playbackController.UpdatePlayer();
            return isNeedRepaint;
        }

        internal void DrawPreviewFrameRate(Rect rect) {
            var layout = GUILayout.Width(40);
            GUILayout.Label(this.previewWindowFps.ToString("0.00"), "preLabel", layout);
        }

        internal bool IsRectPessed(Rect targetRect) {
            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) {
                if (targetRect.Contains(Event.current.mousePosition)) {
                    return true;
                }
            }
            return false;
        }

        internal void SeekedPlayer(float seekPosition) {
            playbackController.Seek(seekPosition);
        }
    }
}

/** @} */
