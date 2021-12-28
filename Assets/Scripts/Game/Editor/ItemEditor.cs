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
                HorizontalLine(Color.gray);
                item.itemName = EditorGUILayout.TextField("Item name: ", item.itemName);
                HorizontalLine(Color.gray);
                item.icon = (Sprite) EditorGUILayout.ObjectField("Sprite:", item.icon, typeof(Sprite), allowSceneObjects: true);
                HorizontalLine(Color.gray);
                amountOpen = EditorGUILayout.Foldout(amountOpen, "Amount", true);
                if (amountOpen)
                {
                    item.amount.SetValue(EditorGUILayout.IntSlider("Value: ", (int)item.amount.Value, (int)item.amount.Min, (int)item.amount.Max));
                    var min = EditorGUILayout.IntSlider("Min: ", (int)item.amount.Min, 0, (int)item.amount.Max);
                    var max = EditorGUILayout.IntSlider("Max: ", (int)item.amount.Max, (int)item.amount.Min, (int)item.amount.MaxCount);
                    item.amount.MaxCount = EditorGUILayout.IntField("Max value: ", (int)item.amount.MaxCount);
                    item.amount.SetClamp(min, max);
                }

                HorizontalLine(Color.gray);
            
                item.itemType = (ItemType) EditorGUILayout.EnumPopup("Type: ", item.itemType);
                HorizontalLine(Color.gray);

                keysOpen = EditorGUILayout.Foldout(keysOpen, "Keys Values", true);
                if (keysOpen)
                {
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
                            }

                            if (GUILayout.Button("-"))
                            {
                                item.keysData.RemoveAt(i);
                                break;
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("Add"))
                    {
                        item.keysData.Add(new KeyPair());
                    }
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }
    
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
}
