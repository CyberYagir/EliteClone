using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Game;
using Core.Garage;
using UnityEditor;
using UnityEngine;

public class TradeWindow : EditorWindow
{
    private TradeManager manager;
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        TradeWindow window = (TradeWindow)EditorWindow.GetWindow(typeof(TradeWindow));
        window.Show();
    }

    private void OnGUI()
    {
        if (IsManager())
        {
            if (ItemsManager.IsInited())
            {
                DrawSides();
            }
            else
            {
                ItemsManager.Init();
            }
        }
    }

    public void DrawSides()
    {
        windowScrollView = EditorGUILayout.BeginScrollView(windowScrollView, false, false);
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    LeftSide();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.MaxWidth(250));
                {
                    RightSide();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

        }
        EditorGUILayout.EndScrollView();
    }

    public Vector2 windowScrollView;

    public void LeftSide()
    {

        for (int i = 0; i < manager.staticOffers.Count; i++)
        {
            GUILayout.BeginHorizontal("Window");
            {
                GUILayout.BeginVertical();
                {
                    if (manager.staticOffers[i].item == null)
                    {
                        manager.staticOffers.RemoveAt(i);
                        break;
                    }
                    GUILayout.Label(manager.staticOffers[i].item.id.idname);
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(-20);
                        GUILayout.BeginVertical();
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUI.enabled = false;
                                EditorGUILayout.ObjectField("", manager.staticOffers[i].item.icon, typeof(Sprite), false, GUILayout.MaxWidth(100));
                                GUI.enabled = true;
                                if (manager.staticOffers[i].costLevel == null)
                                {
                                    manager.staticOffers[i].costLevel = new AnimationCurve();
                                }
                                manager.staticOffers[i].costLevel = EditorGUILayout.CurveField("Cost Curve: ", manager.staticOffers[i].costLevel, GUILayout.Height(60), GUILayout.Width(300));
                                GUILayout.BeginVertical();
                                {
                                    EditorGUILayout.MinMaxSlider("Cost",ref manager.staticOffers[i].minCost, ref manager.staticOffers[i].maxCost, 1, 100);
                                    manager.staticOffers[i].minCost = Mathf.RoundToInt(manager.staticOffers[i].minCost);
                                    manager.staticOffers[i].maxCost = Mathf.RoundToInt(manager.staticOffers[i].maxCost);
                                    GUILayout.Label("Cost: " + manager.staticOffers[i].minCost + "/" + manager.staticOffers[i].maxCost);
                                }
                                GUILayout.EndHorizontal();
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                    if (GUILayout.Button("Drop"))
                    {
                        manager.staticOffers.RemoveAt(i);
                        break;
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }

    public void RightSide()
    {
        var n = ItemsManager.GetItemList();
        for (int i = 0; i < n.Count; i++)
        {
            GUILayout.BeginHorizontal();
            {
                if (manager.staticOffers.Find(x => x.item == n[i]) != null)
                {
                    GUI.enabled = false;
                }
                if (GUILayout.Button("+", GUILayout.MaxWidth(30)))
                {
                    AddToOffer(n[i]);
                }
                GUILayout.Label(i.ToString("000"), GUILayout.MaxWidth(30));
                GUILayout.Label("| " + n[i].id.idname);
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
        }        
    }

    private void AddToOffer(Item item)
    {
        manager.staticOffers.Add(new TradeManager.TradeOffer(){item = item});
    }


    public bool IsManager()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<TradeManager>();
            if (manager == null) return false;
        }
        EditorUtility.SetDirty(manager);
        return true;
    }
}
