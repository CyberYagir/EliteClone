using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    private Item item;
    public static bool amountOpen;
    public static int maxRange = 10;
    private void OnEnable()
    {
        item = target as Item;
    }

    private void OnDisable()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public override void OnInspectorGUI()
    {
        
        GUILayout.BeginHorizontal();
        {
            GUI.enabled = false;
            GUILayout.Label("ID: ");
            EditorGUILayout.IntField(item.id.id);
            EditorGUILayout.TextField(item.id.idname);
            GUI.enabled = true;
        }
        GUILayout.EndHorizontal();
        
        EditorGUI.BeginChangeCheck();
        
        HorizontalLine(Color.gray);
        item.itemName = EditorGUILayout.TextField("Item name: ", item.itemName);
        HorizontalLine(Color.gray);
        item.icon = (Sprite)EditorGUILayout.ObjectField("Sprite:", item.icon, typeof(Sprite), allowSceneObjects: true);
        HorizontalLine(Color.gray);
        amountOpen = EditorGUILayout.Foldout(amountOpen, "Amount", true);
        if (amountOpen)
        {
            item.amount.SetValue(EditorGUILayout.Slider("Value: ", item.amount.Value, item.amount.Min, item.amount.Max));
            var min = EditorGUILayout.Slider("Min: ", item.amount.Min, 0, item.amount.Max);
            var max = EditorGUILayout.Slider("Max: ", item.amount.Max, item.amount.Min, maxRange);
            maxRange = EditorGUILayout.IntField("Max value: ", maxRange);

            item.amount.SetClamp(min,max);
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }
    
    static void HorizontalLine ( Color color, float space = 20) {
        
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
