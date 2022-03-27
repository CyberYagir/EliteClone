using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TweaksEditor : Editor
{
    public static void HorizontalLine ( Color color, float space = 20) {
        
        GUIStyle horizontalLine;
        horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset( 0, 0, 4, 4 );
        horizontalLine.fixedHeight = 1;
        GUILayout.Space(space/2f);
        var c = GUI.color;
        GUI.color = color;
        GUILayout.Box( GUIContent.none, horizontalLine );
        GUI.color = c;
        
        GUILayout.Space(space/2f);
    }
}
