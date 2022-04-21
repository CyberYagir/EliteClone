using System;
using System.Collections.Generic;
using System.Text;
using Core.Location;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Core.Game
{
    [Serializable]
    public class ValueLimit
    {
        [SerializeField] public float value;
        [SerializeField] float minValue, maxValue;
        public float Value => value;
        public float Min => minValue;
        public float Max => maxValue;
        [HideInInspector]
        public float MaxCount = 10;
        public void SetClamp(int min, int max)
        {
            minValue = min;
            maxValue = max;
            if (minValue > maxValue)
            {
                maxValue = minValue;
            }
        }

        public ref float GetValue()
        {
            return ref value;
        }
    

        public void SetMinZero()
        {
            minValue = 0;
        }
        public void SetValue(int val)
        {
            value = val;
            Clamp();
        }
        public void SetValue(float val)
        {
            value = val;
            Clamp();
        }
        public void AddValue(int val)
        {
            value += val;
            Clamp();
        }
        public void SubValue(int val)
        {
            value -= val;
            Clamp();
        }
        public void SubValue(float val)
        {
            value -= val;
            Clamp();
        }
        public void AddValue(float val)
        {
            value += val;
            Clamp();
        }

        void Clamp()
        {
            value = Mathf.Clamp(value, minValue, maxValue);
        }
    }

    [Serializable]
    public class IDTruple
    {
        public int id;
        public string idname;

        public IDTruple()
        {
        
        }
    
        public IDTruple(string nameID)
        {
            idname = nameID;
            int uniqSeed = NamesHolder.StringToSeed(nameID);
            id = int.Parse(new Random(uniqSeed).Next(-999999, 999999).ToString("0000000"));
        }
    }

    public enum KeyPairValue
    {
        Damage, Cooldown, Mass, Level, Value, Energy, Mineral
    }

    public enum ItemType
    {
        None, Weapon, Shields, Armor, Cooler, Generator
    }
    public enum KeyPairType
    {
        Int, String, Float, MineralType, Object
    }

    public enum MineralType
    {
        Al, Fe, Au, Mg, Mn, Cu, Ni, Sn, Pt, Ag, Cr, Zn, Rock, Slag, Dust
    }

    [Serializable]
    public class KeyPair
    {
        public KeyPairValue KeyPairValue;
        public KeyPairType  KeyPairType;
        public string customName = "";
        public float num = 1 ;
        public string str;
        public Object obj;

        public object GetValue()
        {
            switch (KeyPairType)
            {
                case KeyPairType.String:
                    return str;
                case KeyPairType.Object:
                    return obj;
                default:
                    return num;
            }
        }
    }

    [CreateAssetMenu(fileName = "", menuName = "Game/Item", order = 1)]
    public class Item : ScriptableObject
    {
        public IDTruple id;
        public string itemName;
        public Sprite icon;
        public ValueLimit amount;
        public ItemType itemType;
        public List<KeyPair> keysData;

        public object GetKeyPair(KeyPairValue value)
        {
            var fided = keysData.Find(x => x.KeyPairValue == value);
            if (fided != null)
            {
                return fided.GetValue();
            }
            return 0f;
        }

        public bool IsHaveKeyPair(KeyPairValue value)
        {
            return keysData.Find(x => x.KeyPairValue == value) != null;
        }

        public Item Clone()
        {
            var clone = Instantiate(this);
            clone.amount.SetMinZero();
            return clone;
        } 
    }
}