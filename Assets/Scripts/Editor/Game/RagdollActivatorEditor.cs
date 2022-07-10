using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RagdollActivator))]
public class RagdollActivatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Fill"))
        {
            var trg = target as RagdollActivator;
            trg.GetAll();
        }
    }
}
