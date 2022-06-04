using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using DG.DemiEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EditTextWindow : EditorWindow
{
    public static EditTextWindow Instance;
    public static void Open()
    {
        Instance = GetWindow<EditTextWindow>();
        Instance.titleContent = new GUIContent("Edit Value");
        var size = new Vector2(300, 270);
        Instance.maxSize = size;
        Instance.minSize = size;
    }

    public static void Close()
    {
        if (Instance != null)
        {
            ((EditorWindow) Instance).Close();
        }
    }

    public static EditTextWindow GetInstance()
    {
        return Instance;
    }

    public void SetData(string text)
    {
        this.text = text;
    }

    public UnityEvent<string> ChangeCallback = new Event<string>();
    public string text;
    private void OnGUI()
    {
        text = EditorGUILayout.TextField(text, GUILayout.MinHeight(220), GUILayout.MaxHeight(225));
        if (GUILayout.Button("Save"))
        {
            ChangeCallback.Invoke(text);
            Close();
        }
        if (GUILayout.Button("Close"))
        {
            Close();
        }
    }

    

    private void OnDisable()
    {
        
    }
}
