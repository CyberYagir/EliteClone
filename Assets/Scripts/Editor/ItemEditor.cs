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
    public static bool keysOpen;
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
        {
            HorizontalLine(Color.gray);
            item.itemName = EditorGUILayout.TextField("Item name: ", item.itemName);
            HorizontalLine(Color.gray);
            item.icon = (Sprite) EditorGUILayout.ObjectField("Sprite:", item.icon, typeof(Sprite), allowSceneObjects: true);
            HorizontalLine(Color.gray);
            amountOpen = EditorGUILayout.Foldout(amountOpen, "Amount", true);
            if (amountOpen)
            {
                item.amount.SetValue(EditorGUILayout.IntSlider("Value: ", item.amount.Value, item.amount.Min, item.amount.Max));
                var min = EditorGUILayout.IntSlider("Min: ", item.amount.Min, 0, item.amount.Max);
                var max = EditorGUILayout.IntSlider("Max: ", item.amount.Max, item.amount.Min, item.amount.MaxCount);
                item.amount.MaxCount = EditorGUILayout.IntField("Max value: ", item.amount.MaxCount);
                item.amount.SetClamp(min, max);
            }

            HorizontalLine(Color.gray);

            keysOpen = EditorGUILayout.Foldout(keysOpen, "Keys Values", true);
            if (keysOpen)
            {
                for (int i = 0; i < item.keys.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    {
                        item.keys[i].KeyPairValue = (KeyPairValue) EditorGUILayout.EnumPopup(item.keys[i].KeyPairValue);
                        item.keys[i].KeyPairType = (KeyPairType) EditorGUILayout.EnumPopup(item.keys[i].KeyPairType);
                        switch (item.keys[i].KeyPairType)
                        {
                            case KeyPairType.Int:
                                int intVal = 0;
                                try
                                {
                                    intVal = int.Parse(item.keys[i].value.ToString());
                                }
                                catch (Exception e)
                                {
                                    intVal = 0;
                                }

                                item.keys[i].value = EditorGUILayout.IntField(intVal);
                                break;
                            case KeyPairType.Float:
                                float fltVal = 0;
                                try
                                {
                                    fltVal = float.Parse(item.keys[i].value.ToString());
                                }
                                catch (Exception e)
                                {
                                    fltVal = 0;
                                }

                                item.keys[i].value = EditorGUILayout.FloatField(fltVal);
                                break;
                            case KeyPairType.String:
                                item.keys[i].value = EditorGUILayout.TextField(item.keys[i].value.ToString().Trim()).Trim();
                                break;
                        }

                        if (GUILayout.Button("-"))
                        {
                            item.keys.RemoveAt(i);
                            break;
                        }
                    }
                    GUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add"))
                {
                    item.keys.Add(new KeyPair());
                }
            }
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
