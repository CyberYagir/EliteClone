using System.Collections;
using System.Collections.Generic;
using Core.Garage;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TradeManager))]
public class TradeEditor : TweaksEditor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open window"))
        {
            TradeWindow.Init();
        }
        
    }
}
