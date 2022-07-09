using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Location;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LocationPoint))]
public class LocationPointEditor : Editor
{
    private LocationPoint lp;

    private void OnEnable()
    {
        lp = target as LocationPoint;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (lp.Location == LocationPoint.LocationType.Scene)
        {
            var oldScene = lp.Scene;
            lp.SetScene((Scenes)EditorGUILayout.EnumPopup("Scene: ", lp.Scene));
            if (oldScene != lp.Scene)
            {
                EditorUtility.SetDirty(lp);
            }
        }
    }
}
