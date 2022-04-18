using System.Collections.Generic;
using Core.Game;
using Core.PlayerScripts;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : UnityEditor.Editor
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
            EditorUtility.SetDirty(item);
            AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
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
                TweaksEditor.HorizontalLine(Color.gray);
                item.itemName = EditorGUILayout.TextField("Item name: ", item.itemName);
                TweaksEditor.HorizontalLine(Color.gray);
                item.icon = (Sprite) EditorGUILayout.ObjectField("Sprite:", item.icon, typeof(Sprite), allowSceneObjects: true);
                TweaksEditor.HorizontalLine(Color.gray);
                amountOpen = EditorGUILayout.Foldout(amountOpen, "Amount", true);
                if (amountOpen)
                {
                    item.amount.SetValue(EditorGUILayout.IntSlider("Value: ", (int)item.amount.Value, (int)item.amount.Min, (int)item.amount.Max));
                    var min = EditorGUILayout.IntSlider("Min: ", (int)item.amount.Min, 0, (int)item.amount.Max);
                    var max = EditorGUILayout.IntSlider("Max: ", (int)item.amount.Max, (int)item.amount.Min, (int)item.amount.MaxCount);
                    item.amount.MaxCount = EditorGUILayout.IntField("Max value: ", (int)item.amount.MaxCount);
                    item.amount.SetClamp(min, max);
                }

                TweaksEditor.HorizontalLine(Color.gray);
            
                item.itemType = (ItemType) EditorGUILayout.EnumPopup("Type: ", item.itemType);
                TweaksEditor.HorizontalLine(Color.gray);

                keysOpen = EditorGUILayout.Foldout(keysOpen, "Keys Values", true);
                if (keysOpen)
                {
                    if (item.keysData == null)
                    {
                        InitItem(item);
                    }
                    
                    for (int i = 0; i < item.keysData.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            item.keysData[i].KeyPairValue = (KeyPairValue) EditorGUILayout.EnumPopup(item.keysData[i].KeyPairValue);
                            EditorGUI.BeginChangeCheck();
                            {
                                item.keysData[i].KeyPairType = (KeyPairType) EditorGUILayout.EnumPopup(item.keysData[i].KeyPairType);
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                if (item.keysData[i].KeyPairType == KeyPairType.Int)
                                {
                                    item.keysData[i].num = Mathf.Round(item.keysData[i].num);
                                }
                            }

                            if (item.keysData[i].customName == "")
                            {
                                item.keysData[i].customName = EditorGUILayout.TextField(item.keysData[i].customName, GUILayout.Width(10));
                            }
                            
                            switch (item.keysData[i].KeyPairType)
                            {
                                case KeyPairType.Int:
                                    item.keysData[i].num = EditorGUILayout.IntField((int) item.keysData[i].num);
                                    break;
                                case KeyPairType.Float:
                                    item.keysData[i].num = EditorGUILayout.FloatField(item.keysData[i].num);
                                    break;
                                case KeyPairType.String:    
                                    item.keysData[i].str = EditorGUILayout.TextField(item.keysData[i].str).Trim();
                                    break;
                                case KeyPairType.MineralType:
                                    item.keysData[i].num = (int)((MineralType)EditorGUILayout.EnumPopup((MineralType)((int)item.keysData[i].num)));
                                    break;
                                case KeyPairType.Object:
                                    item.keysData[i].obj = EditorGUILayout.ObjectField(item.keysData[i].obj, typeof(Object));
                                    break;
                            }

                            if (GUILayout.Button("-"))
                            {
                                item.keysData.RemoveAt(i);
                                break;
                            }
                        }
                        GUILayout.EndHorizontal();
                        if (item.keysData[i].customName != "")
                        {
                            item.keysData[i].customName = EditorGUILayout.TextField("Custom Name: ", item.keysData[i].customName);
                        }
                    }

                    if (GUILayout.Button("Add"))
                    {
                        item.keysData.Add(new KeyPair());
                    }
                    if (GUILayout.Button("Reset"))
                    {
                        item.keysData = null;
                    }
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }
    
        
        public void InitItem(Item item){
            item.keysData = new List<KeyPair>();
            item.keysData.Add(new KeyPair()
            {
                KeyPairType = KeyPairType.Int, KeyPairValue = KeyPairValue.Level, num = 1
            });

            item.keysData.Add(new KeyPair()
            {
                KeyPairType = KeyPairType.Float, KeyPairValue = KeyPairValue.Mass, num = 1f
            });
            
            if (item.itemType == ItemType.Weapon)
            {
                item.keysData.Add(new KeyPair()
                {
                    KeyPairType = KeyPairType.Float, KeyPairValue = KeyPairValue.Cooldown, num = 0.2f
                });
                item.keysData.Add(new KeyPair()
                {
                    KeyPairType = KeyPairType.Float, KeyPairValue = KeyPairValue.Damage, num = 1f
                });
                item.keysData.Add(new KeyPair()
                {
                    KeyPairType = KeyPairType.Float, KeyPairValue = KeyPairValue.Energy, num = -1f
                });
            }

            if (item.itemType == ItemType.Generator)
            {
                item.keysData.Add(new KeyPair()
                {
                    KeyPairType = KeyPairType.Float, KeyPairValue = KeyPairValue.Energy, num = 5f
                });
            }
                        
            if (item.itemType == ItemType.Armor || item.itemType == ItemType.Cooler)
            {
                item.keysData.Add(new KeyPair()
                {
                    KeyPairType = KeyPairType.Float, KeyPairValue = KeyPairValue.Value, num = 10f
                });
            }

        }

    }
    
    
}
