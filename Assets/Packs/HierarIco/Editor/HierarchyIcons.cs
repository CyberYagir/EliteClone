using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

[InitializeOnLoad]
class HierarchyIcons
{
    private static IconsList list;

    static HierarchyIcons()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static Vector2 offset = new Vector2(18, 0);


    private static Dictionary<string, GUIContent> iconsNames = new Dictionary<string, GUIContent>();
    private static GUIContent folder;
    private static GUIContent tmpText;


    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        if (Event.current.type == EventType.Repaint)
        {
            if (CreateSingleton())
            {
                var obj = EditorUtility.InstanceIDToObject(instanceID);
                if (obj != null)
                {
                    var go = (obj as GameObject);
                    selectionRect.position += offset;
                    Rect offsetRect = new Rect(selectionRect.position, selectionRect.size);

                    var iconPos = GetIconPos(selectionRect);
                    bool isfolder = false;
                    if (go.GetComponent<Folder>())
                    {
                        go.transform.localPosition = Vector3.zero;
                        GUI.Label(iconPos, folder);
                        isfolder = true;
                    }
                    else if (go.TryGetComponent(out Light light))
                    {
                        GUI.Label(iconPos, DrawLightIcon(light.type));
                    }
                    else if (go.transform.name.ToLower() == "gamemanager")
                    {
                        GUI.Label(iconPos, EditorGUIUtility.IconContent("GameManager Icon"));
                    }
                    else if (go.GetComponent<TMP_Text>())
                    {
                        iconPos.size = Vector2.one * 18;
                        iconPos.position += new Vector2(3, 2.5f);
                        GUI.Label(iconPos, tmpText);
                    }else
                    {
                        for (int i = 0; i < list.icons.Count; i++)
                        {

                            var type = list.icons[i];
                            if (go.GetComponent(type))
                            {
                                iconPos.size = Vector2.one * 20;
                                iconPos.position -= Vector2.down * 2.5f;
                                iconPos.position += Vector2.right * 2f;
                                var key = $"d_{list.icons[i]} Icon";
                                if (!iconsNames.ContainsKey(key))
                                {
                                    var icon = EditorGUIUtility.IconContent(key);
                                    if (icon != null)
                                    {
                                        iconsNames.Add(key, icon);
                                    }
                                    else
                                    {
                                        list.icons.RemoveAt(i);
                                        return;
                                    }
                                }
                                else
                                {
                                    GUI.Label(iconPos, iconsNames[key]);
                                }

                                return;
                            }
                        }
                    }

                    if (list.allEmptyFolders && !isfolder && go.transform.localPosition == Vector3.zero)
                    {
                        if (go.transform.GetComponents<Component>().Length == 1)
                        {
                            go.AddComponent<Folder>();
                        }
                    }
                }
            }
        }
    }

    public static bool CreateSingleton()
    {
        if (list == null)
        {
            var find = AssetDatabase.FindAssets("t:IconsList");
            if (find.Length != 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(find[0]);
                list = AssetDatabase.LoadAssetAtPath<IconsList>(path);
                folder = EditorGUIUtility.IconContent("Folder Icon");


                var tmptextIcons = AssetDatabase.FindAssets("TMP - Text Component Icon");
                if (tmptextIcons.Length != 0)
                {
                    var tmpIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(tmptextIcons[0]));
                    tmpText = new GUIContent(tmpIcon);
                }
                
            }
        }

        return list != null;
    }
    
    public static GUIContent DrawLightIcon(LightType lightType)
    {
        var type = "";
        switch (lightType)
        {
            case LightType.Directional:
                type = "d_DirectionalLight Icon";
                break;
            case LightType.Spot:
                type = "d_Spotlight Icon";
                break;
            default:
                type = "d_Light Icon";
                break;

        }

        return EditorGUIUtility.IconContent(type);
    }

    public static Rect GetIconPos(Rect selectionRect)
    {
        var iconPos = selectionRect;
        iconPos.position += new Vector2(selectionRect.width / 1.25f, 0);
        iconPos.position -= Vector2.up * 5;
        iconPos.size = new Vector2(25, 25);

        return iconPos;
    }

}