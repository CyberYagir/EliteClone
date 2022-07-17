using System.Collections;
using System.Collections.Generic;
using Core.Init;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InitChangelog))]
public class InitChangelogEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("GenerateJson"))
        {
           Debug.Log(JsonConvert.SerializeObject((target as InitChangelog).versions));
        }
    }
}
