using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    [System.Serializable]
    public class ShipVariables
    {
        public float ZRotSpeed, XRotSpeed, YRotSpeed;
        public float maxSpeedUnits, speedUpMultiplier;
        public int maxCargoWeight;
    }
    [System.Serializable]
    public class ShipClaped
    {
        public ItemShip.ShipValuesTypes name;
        public float value;

        public float max;

        public ShipClaped()
        {
            
        }
        public ShipClaped(ItemShip.ShipValuesTypes name, float value, float max)
        {
            this.name = name;
            this.value = value;
            this.max = max;
        }

        public void Clamp()
        {
            if (value > max)
            {
                value = max;
            }
        }
    }

    [System.Serializable]
    public class Slot
    {
        public int uid;
        public ItemType slotType;
        public int slotLevel = 1;
        [JsonIgnore]
        public Item current;

        public int button = -1;

        public Cargo.ItemData itemData = null;

        public Slot()
        {
            
        }
        
        public void Save()
        {
            if (current != null)
            {
                itemData = new Cargo.ItemData() {idName = current.id.idname, value = current.amount.Value};
            }
        }

        public void Load()
        {
            if (itemData != null)
            {
                current = ItemsManager.GetItem(itemData);
            }
        }

    }
    [CreateAssetMenu(fileName = "", menuName = "Game/Ship", order = 1)]
    public class ItemShip : ScriptableObject
    {
        public string shipName;
        public GameObject shipModel;
        public GameObject shipCabine;
        public GameObject shipWreckage;
        public ShipVariables data;
        public List<Slot> slots;
        public Dictionary<ShipValuesTypes, ShipClaped> shipValues;
        public Event OnChangeShipData = new Event();
        public int shipID;
        public enum ShipValuesTypes
        {
            Fuel, Health, Shields, Temperature
        }

        public List<ShipClaped> shipValuesList = new List<ShipClaped> { new ShipClaped(ShipValuesTypes.Fuel, 1000, 1000), new ShipClaped(ShipValuesTypes.Health, 100, 100), new ShipClaped(ShipValuesTypes.Shields, 100, 100), new ShipClaped(ShipValuesTypes.Temperature, 100, 100)};
        public ItemShip Clone()
        {
            var clone = Instantiate(this);
            clone.ValuesToDictionary();
            clone.shipID = new Random(DateTime.Now.Millisecond * DateTime.Now.Second * DateTime.Now.Hour * DateTime.Now.Day).Next(100000000, 999999999);
            return clone;
        }

        public void ReplaceSlotItem(Item item, int slotID, Cargo cargo)
        {
            var slot = slots.Find(x => x.uid == slotID);
            if (slot != null)
            {
                if (item.itemType == slot.slotType)
                {
                    var oldItem = slot.current;
                    cargo.AddItem(oldItem);
                    var removed = cargo.RemoveItem(item.id.idname, 1, true);
                    slot.current = removed;
                }
                OnChangeShipData.Invoke();
            }
        } 
        public float CalcMass()
        {
            var mass = 0f;
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].current.IsHaveKeyPair(KeyPairValue.Mass))
                {
                    mass += (float) slots[i].current.GetKeyPair(KeyPairValue.Mass);
                }
            }

            return mass;
        }
        public void ValuesToDictionary()
        {
            shipValues = new Dictionary<ShipValuesTypes, ShipClaped>();
            for (int i = 0; i < shipValuesList.Count; i++)
            {
                if (!shipValues.ContainsKey(shipValuesList[i].name))
                {
                    shipValues.Add(shipValuesList[i].name, shipValuesList[i]);
                }
            }
        }

        public ShipClaped GetValue(ShipValuesTypes name)
        {
            if (shipValues.ContainsKey(name))
            {
                return shipValues[name];
            }
            Debug.LogError($"{name} not found in ship {shipName}");
            return new ShipClaped();
        }
        public ShipData SaveShip()
        {
            return new ShipData(this);
        }
    }



    public class ShipData
    {
        public int shipID;
        public string shipName;
        public List<ShipClaped> valuesList = new List<ShipClaped>();
        public List<Slot> slots;

        public ShipData()
        {
            
        }

        public ShipData(ItemShip item)
        {
            shipName = item.shipName;
            foreach (var value in item.shipValues)
            {
                valuesList.Add(value.Value);
            }

            foreach (var slot in item.slots)
            {
                slot.Save();
            }

            shipID = item.shipID;
            slots = item.slots;
        }

        public ItemShip GetShip()
        {
            var ship = ItemsManager.GetShipItem(shipName);
            ship.shipValuesList = valuesList;
            ship.ValuesToDictionary();
            foreach (var slot in slots)
            {
                slot.Load();
            }

            ship.shipID = shipID;
            ship.slots = slots;

            return ship;
        }
    }
}