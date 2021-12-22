using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
[CanEditMultipleObjects]
[CustomEditor(typeof(ItemList))]
public class ItemListEditor : Editor
{
    private ItemList itemsList;

    private void OnEnable()
    {
        itemsList = target as ItemList;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get Items"))
        {
            var items = Resources.LoadAll<Item>("").ToList();
            items = items.OrderBy(x => File.GetCreationTime(AssetDatabase.GetAssetPath(x) + ".meta")).ToList();
            var idDir = new List<int>();
            var nameDir = new List<string>();
            
            foreach (var obj in items)
            {
                int trys = 0;
                obj.id = null;
                while (obj.id == null || idDir.Contains(obj.id.id) || nameDir.Contains(obj.id.idname))
                {
                    obj.id = new IDTruple(obj.itemName.Replace(" ", "_").ToLower().Trim() + (trys != 0 ? ":" + trys : ""));
                    trys++;
                }
                idDir.Add(obj.id.id);
                nameDir.Add(obj.id.idname);
                EditorUtility.SetDirty(obj);
            }
            itemsList.SetList(items);
            EditorUtility.SetDirty(itemsList);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
