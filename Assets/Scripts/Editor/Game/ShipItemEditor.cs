using System;
using System.Linq;
using Core.Game;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using Slot = Core.Game.Slot;

namespace Game.Editor
{
    [CustomEditor(typeof(ItemShip))]
    public class ShipItemEditor : UnityEditor.Editor
    {
        UnityEditor.Editor gameObjectEditor;
        private static bool drawStats, drawSlots;
        private ItemShip ship;
        private GameObject mesh;

        private void OnEnable()
        {
            ship = target as ItemShip;
            if (!Application.isPlaying)
            {
                mesh = Instantiate(ship.shipModel);
                mesh.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(ship);
            AssetDatabase.SaveAssets();
            if (gameObjectEditor)
                gameObjectEditor.ResetTarget();
            DestroyImmediate(gameObjectEditor);
            DestroyMesh();
        }

        private void OnDestroy()
        {
            if (gameObjectEditor)
                gameObjectEditor.ResetTarget();
            DestroyImmediate(gameObjectEditor);
            DestroyMesh();
        }

        public void DestroyMesh()
        {
            if (mesh)
                DestroyImmediate(mesh.gameObject);
        }


        public override void OnInspectorGUI()
        {
            var oldLengh = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 55;

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    ship.shipName = EditorGUILayout.TextField("Name:", ship.shipName);
                    GUI.enabled = false;
                    EditorGUILayout.IntField("UID:", ship.shipID);
                    GUI.enabled = true;
                    ship.shipModel = (GameObject) EditorGUILayout.ObjectField("Ship:", ship.shipModel, typeof(GameObject));
                    ship.shipCabine = (GameObject) EditorGUILayout.ObjectField("Cabine:", ship.shipCabine, typeof(GameObject));
                    ship.shipWreckage = (GameObject) EditorGUILayout.ObjectField("Wreckage:", ship.shipWreckage, typeof(GameObject));

                    TweaksEditor.HorizontalLine(Color.gray);
                    EditorGUILayout.LabelField("Data: ", EditorStyles.boldLabel);
                    EditorGUIUtility.labelWidth = 80;
                    ship.data.XRotSpeed = EditorGUILayout.FloatField("X Speed: ", ship.data.XRotSpeed);
                    ship.data.YRotSpeed = EditorGUILayout.FloatField("Y Speed: ", ship.data.YRotSpeed);
                    ship.data.ZRotSpeed = EditorGUILayout.FloatField("Z Speed: ", ship.data.ZRotSpeed);
                    ship.data.maxSpeedUnits = EditorGUILayout.FloatField("Max Speed: ", ship.data.maxSpeedUnits);
                    ship.data.speedUpMultiplier = EditorGUILayout.FloatField("Speed Up: ", ship.data.speedUpMultiplier);
                    ship.data.maxCargoWeight = EditorGUILayout.IntField("Cargo Tons: ", ship.data.maxCargoWeight);
                }
                GUILayout.EndHorizontal();
            }
            DrawPreview();
            GUILayout.EndHorizontal();

            TweaksEditor.HorizontalLine(Color.gray);
            drawStats = EditorGUILayout.Foldout(drawStats, "Stats", true);
            if (drawStats)
            {
                if (ship.shipValues == null)
                {
                    for (int i = 0; i < ship.shipValuesList.Count; i++)
                    {
                        var res = DrawEditor(ship.shipValuesList[i]);
                        if (res != null)
                        {
                            ship.shipValuesList[i] = res;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    foreach (var it in ship.shipValues)
                    {
                        var res = DrawEditor(it.Value);
                        // if (res != null)
                        // {
                        //     ship.shipValues[it.Key] = res;
                        // }
                        // else
                        // {
                        //     return;
                        // }   
                    }
                }

                if (GUILayout.Button("Add"))
                {
                    ship.shipValuesList.Add(new ShipClaped());
                }
            }

            TweaksEditor.HorizontalLine(Color.gray);
            drawSlots = EditorGUILayout.Foldout(drawSlots, "Slots", true);
            if (drawSlots)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Unc Slot ID");
                    EditorGUILayout.LabelField("Max Level");
                    EditorGUILayout.LabelField("Type");
                }
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < ship.slots.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    ship.slots[i].uid = EditorGUILayout.IntField(ship.slots[i].uid);
                    ship.slots[i].slotLevel = EditorGUILayout.IntField(ship.slots[i].slotLevel);
                    ship.slots[i].slotType = (ItemType) EditorGUILayout.EnumPopup(ship.slots[i].slotType);

                    EditorGUI.BeginChangeCheck();
                    {
                        ship.slots[i].current = (Item) EditorGUILayout.ObjectField(ship.slots[i].current, typeof(Item));
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (ship.slots[i].current != null)
                        {
                            if (ship.slots[i].current.itemType != ship.slots[i].slotType)
                            {
                                ship.slots[i].current = null;
                            }
                        }
                    }

                    if (GUILayout.Button("-"))
                    {
                        ship.slots.RemoveAt(i);
                        break;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add"))
                {
                    ship.slots.Add(new Slot() {uid = ship.slots.Count});
                }
            }

            TweaksEditor.HorizontalLine(Color.gray);
            EditorGUIUtility.labelWidth = oldLengh;
        }

        public ShipClaped DrawEditor(ShipClaped clamped)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);
                TweaksEditor.HorizontalLine(Color.gray, 30);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                var old = clamped.name;
                EditorGUI.BeginChangeCheck();
                {
                    clamped.name = (ItemShip.ShipValuesTypes) EditorGUILayout.EnumPopup(clamped.name, GUILayout.Width(60));
                }
                if (EditorGUI.EndChangeCheck())
                {
                    if (ship.shipValuesList.Find(x => x.name == clamped.name && x != clamped) != null)
                    {
                        clamped.name = old;
                    }
                }

                EditorGUILayout.LabelField("Value");
                EditorGUILayout.LabelField("Max");
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("-", GUILayout.Width(65)))
                {
                    ship.shipValuesList.Remove(clamped);
                    return null;
                }

                clamped.value = EditorGUILayout.FloatField(clamped.value);
                clamped.max = EditorGUILayout.FloatField(clamped.max);
            }
            GUILayout.EndHorizontal();
            return clamped;
        }

        public void DrawPreview()
        {
            if (mesh)
            {
                EditorGUI.BeginChangeCheck();

                if (EditorGUI.EndChangeCheck())
                {
                    if (gameObjectEditor != null) DestroyImmediate(gameObjectEditor);
                }

                GUIStyle bgColor = new GUIStyle();

                bgColor.normal.background = EditorGUIUtility.whiteTexture;

                if (gameObjectEditor == null)
                {
                    gameObjectEditor = UnityEditor.Editor.CreateEditor(mesh.gameObject);
                }
                else
                {
                    try
                    {
                        gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(200, 200), bgColor);
                    }
                    catch (Exception e)
                    {
                        Selection.objects = null;
                    }
                }
            }
        }
    }
}
