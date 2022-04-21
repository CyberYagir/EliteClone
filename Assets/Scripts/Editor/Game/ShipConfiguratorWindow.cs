using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Game;
using UnityEditor;
using UnityEngine;


public class ShipConfiguratorWindow : EditorWindow
{
    private List<ItemShip> ships;
    private List<Item> items;
    private List<Item> slotsSelected;
    private ItemShip selected;
    
    [MenuItem("Tools/Ship Constructor")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        ShipConfiguratorWindow window = (ShipConfiguratorWindow)EditorWindow.GetWindow(typeof(ShipConfiguratorWindow));
        window.Show();
    }

    private void OnEnable()
    {
        ships = Resources.LoadAll<ItemShip>("Game").ToList();
        items = Resources.LoadAll<Item>("").ToList();
    }

    private void OnGUI()
    {
        int newSelection = 0;
        EditorGUI.BeginChangeCheck();
        {
            var list = new List<string>();
            for (int i = 0; i < ships.Count; i++)
            {
                list.Add(ships[i].shipName);
            }

            newSelection = EditorGUILayout.Popup("Ship: ", ships.FindIndex(x => x.shipName == selected?.shipName), list.ToArray());
        }
        
        if (EditorGUI.EndChangeCheck())
        {
            selected = ships[newSelection].Clone();
            slotsSelected = new List<Item>();
            for (int i = 0; i < selected.slots.Count; i++)
            {
                slotsSelected.Add(selected.slots[i].current);
            }
        }

        if (selected != null)
        {
            for (int i = 0; i < selected.slots.Count; i++)
            {
                DrawSlot(selected.slots[i], i);
            }


            var subsEnergy = slotsSelected.FindAll(x => x.itemType != ItemType.Generator);
            var subs = 0f;
            
            for (int i = 0; i < subsEnergy.Count; i++)
            {
                subs += (float)subsEnergy[i].GetKeyPair(KeyPairValue.Energy);
            }
            
            var energy = (float)slotsSelected.Find(x => x.itemType == ItemType.Generator).GetKeyPair(KeyPairValue.Energy);

            GUILayout.Label($"Empty energy: {energy}/{-subs} ({energy - -subs})");
            var mass = 0f;
            for (int i = 0; i < slotsSelected.Count; i++)
            {
                mass += (float)slotsSelected[i].GetKeyPair(KeyPairValue.Mass);
            }
            GUILayout.Label($"Mass: " + mass + "/" + selected.data.maxCargoWeight);
        }
    }

    public void DrawSlot(Slot slot, int applied)
    {
        var findedItems = items.FindAll(x => x.itemType == slot.slotType && (float) x.GetKeyPair(KeyPairValue.Level) <= slot.slotLevel);
        var names = new List<string>();
        foreach (var item in findedItems)
        {
            names.Add(item.itemName);
        }

        var select = 0;
        EditorGUI.BeginChangeCheck();
        {
            EditorGUILayout.BeginHorizontal();
            {
                select = EditorGUILayout.Popup($"Slot {applied}: ", names.FindIndex(x=>x == slotsSelected[applied].itemName), names.ToArray());
                
                GUILayout.Label("Slot Level: " + slot.slotLevel);
                GUILayout.Label("Items Count: " + names.Count);
            }
            EditorGUILayout.EndHorizontal();
        }
        
        if (EditorGUI.EndChangeCheck())
        {
            slotsSelected[applied] = items.Find(x => x.itemName == names[select]);
        }
    }
}
