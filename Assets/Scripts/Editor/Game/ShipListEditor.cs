using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Game;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ShipList))]
    public class ShipListEditor : UnityEditor.Editor
    {
        private ShipList itemsList;

        private void OnEnable()
        {
            itemsList = target as ShipList;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Get Items"))
            {
                var items = Resources.LoadAll<ItemShip>("").ToList();
                items = items.OrderBy(x => File.GetCreationTime(AssetDatabase.GetAssetPath(x) + ".meta")).ToList();
                
                itemsList.SetList(items);
                EditorUtility.SetDirty(itemsList);
            
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
