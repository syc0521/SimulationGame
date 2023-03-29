using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityToolbarExtender;

static class ToolbarStyles
{
	public static readonly GUIStyle commandButtonStyle;

	static ToolbarStyles()
	{
		commandButtonStyle = new GUIStyle("Command")
		{
			fontSize = 15,
			alignment = TextAnchor.MiddleCenter,
			imagePosition = ImagePosition.ImageAbove,
			fontStyle = FontStyle.Normal
		};
	}
}

[InitializeOnLoad]
public class SwitchScene
{
	static SwitchScene()
	{
		ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
	}

	static void OnToolbarGUI()
	{
		GUILayout.FlexibleSpace();

		if (GUILayout.Button(new GUIContent("L", "Launcher"), ToolbarStyles.commandButtonStyle))
		{
			if (EditorApplication.isPlaying) return;
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			EditorSceneManager.OpenScene("Assets/Scenes/Launcher.unity");
		}

		if (GUILayout.Button(new GUIContent("G", "Game"), ToolbarStyles.commandButtonStyle))
		{
			if (EditorApplication.isPlaying) return;
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			EditorSceneManager.OpenScene("Assets/Scenes/Game.unity");
		}

	}
}

#endif
