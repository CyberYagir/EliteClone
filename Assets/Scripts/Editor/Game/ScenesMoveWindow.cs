using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ScenesMoveWindow : EditorWindow
{
    [MenuItem("Tools/Scene Manager")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        ScenesMoveWindow window = (ScenesMoveWindow)EditorWindow.GetWindow(typeof(ScenesMoveWindow));
        window.Show();
    }
    private void OnGUI()
    {
        List<EditorBuildSettingsScene> Scenes = EditorBuildSettings.scenes.ToList();

        for (int i = 0; i < Scenes.Count; i++)
        {
            if (GUILayout.Button(Path.GetFileNameWithoutExtension(Scenes[i].path)))
            {
                EditorApplication.SaveCurrentSceneIfUserWantsTo(); 
                EditorApplication.OpenScene(Scenes[i].path);
            }
        }
    }
}
