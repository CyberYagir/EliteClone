using System;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace Game.Editor
{
    [CustomEditor(typeof(ItemShip))]
    public class ShipItemEditor : UnityEditor.Editor
    {
        UnityEditor.Editor gameObjectEditor;
        static Texture2D previewBackgroundTexture;
        private static bool drawStats, drawSlots;
        private ItemShip ship;
        private GameObject mesh;
        private void OnEnable()
        {
            ship = target as ItemShip;
            mesh = new GameObject();

            if (previewBackgroundTexture == null)
            {
                var path = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("Front_1K_TEX")[0]);
                previewBackgroundTexture = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
            }
        
            Material material = new Material(Shader.Find("HDRP/Lit"));
        
            mesh.AddComponent<MeshRenderer>().material = material;
            mesh.AddComponent<MeshFilter>().mesh = ship.shipModel;
            mesh.hideFlags = HideFlags.HideInHierarchy;
        }
    
        private void OnDisable()
        {
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
                    ship.shipModel = (Mesh) EditorGUILayout.ObjectField("Mesh:", ship.shipModel, typeof(Mesh));
                
                    ItemEditor.HorizontalLine(Color.gray);
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

            ItemEditor.HorizontalLine(Color.gray);
            drawStats = EditorGUILayout.Foldout(drawStats, "Stats", true);
            if (drawStats)
            {
                ship.hp = DrawEditor("HP", ship.hp);
                ship.shields = DrawEditor("Shields", ship.shields);
                ship.fuel = DrawEditor("Fuel", ship.fuel);
                ship.heat = DrawEditor("Heat", ship.heat);
            }
            
            ItemEditor.HorizontalLine(Color.gray);
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

            ItemEditor.HorizontalLine(Color.gray);
            EditorGUIUtility.labelWidth = oldLengh;
        }

        public ShipClaped DrawEditor(string label,  ShipClaped clamped)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);  
                ItemEditor.HorizontalLine(Color.gray, 30);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(label + ":", EditorStyles.boldLabel, GUILayout.Width(60));
                EditorGUILayout.LabelField("Value");
                EditorGUILayout.LabelField("Max");
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(65);
                clamped.value = EditorGUILayout.FloatField(clamped.value);
                clamped.max = EditorGUILayout.FloatField(clamped.max);
            }
            GUILayout.EndHorizontal();
            return clamped;
        }
    
        public void DrawPreview()
        {
            EditorGUI.BeginChangeCheck();
   
            if(EditorGUI.EndChangeCheck())
            {
                if(gameObjectEditor != null) DestroyImmediate(gameObjectEditor);
            }
   
            GUIStyle bgColor = new GUIStyle();
   
            bgColor.normal.background = EditorGUIUtility.whiteTexture;
            if (mesh != null)
            {
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
