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
	[CustomEditor(typeof(CriManaUsmAsset))]
	class CriManaUsmAssetEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CriManaUsmAsset.movieInfo)), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CriManaUsmAsset.assetInfo)), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CriManaUsmAsset.Implementation).ToLowerInvariant()), true);
		}

		public override bool HasPreviewGUI() => true;

		private PreviewWindowController windowController;
		private CriManaUsmAssetPreviewPlayer previewPlayer;
		Texture2D dummyTex = null;

		private void OnEnable()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode) {
				return;
			}
			previewPlayer = new CriManaUsmAssetPreviewPlayer();
			windowController = new PreviewWindowController(previewPlayer);
			windowController.OnEnable();

			previewPlayer.SetAsset(target as CriAssetBase);
		}

		private void OnDisable()
		{
			windowController?.OnDisable();
			windowController = null;
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background)
		{
			UpdatePreview(r);
		}

		public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
		{
			UpdatePreview(r);
		}

		GUIContent _previewTitle = null;
		public override GUIContent GetPreviewTitle()
		{
			return (_previewTitle == null) ? (_previewTitle = new GUIContent(target.name)) : _previewTitle;
		}

		public override void OnPreviewSettings()
		{
			windowController?.OnPreviewSettings(true);
		}

		void UpdatePreview(Rect rect)
		{
			if (Application.isPlaying) {
				return;
			}
			if (dummyTex == null) {
				dummyTex = new Texture2D(1, 1);
			}
			var sequencesRect = windowController.DrawPlaybackSequencesSlider(rect);
			if (windowController.IsRectPessed(sequencesRect)) {
				windowController.SeekedPlayer(Event.current.mousePosition.x / sequencesRect.width);
			}
#if CRI_USM_ASSET_PREVIEW_GUI_DEBUG
			windowController.DrawPreviewFrameRate(rect);
#endif

			var isNeedRepaint = windowController.UpdatePreviewWindow();
			if (isNeedRepaint) {
				Rect manaRect = rect;
				manaRect.height = rect.height - sequencesRect.height;
				manaRect.y += sequencesRect.height;
				manaRect = previewPlayer.ResizeRect(manaRect);
				Graphics.DrawTexture(manaRect, dummyTex, previewPlayer.GetMaterial());
				Repaint();
			}
		}
	}
}

/** @} */
