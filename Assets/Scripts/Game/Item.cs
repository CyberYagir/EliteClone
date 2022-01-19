using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class ValueLimit
{
    [SerializeField] float value;
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

[System.Serializable]
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
        int uniqSeed = 0;
        foreach (var ch in Encoding.ASCII.GetBytes(nameID))
        {
            uniqSeed += ch;
        }
        id = int.Parse(new System.Random(uniqSeed).Next(-999999, 999999).ToString("0000000"));
    }
}

public enum KeyPairValue
{
    Damage, Cooldown, Mass, Level, Value, Energy
}

public enum ItemType
{
    None, Weapon, Shields, Armor, Cooler, Generator
}
public enum KeyPairType
{
    Int, String, Float
}
[System.Serializable]
public class KeyPair
{
    public KeyPairValue KeyPairValue;
    public KeyPairType  KeyPairType;
    public string customName = "";
    public float num = 1 ;
    public string str;

    public object GetValue()
    {
        switch (KeyPairType)
        {
            case KeyPairType.Float:
                return num;
            case KeyPairType.Int:
                return (int) num;
            default:
                return str;
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
            return fided.num;
        }
        return 0f;
    }

    public Item Clone()
    {
        var clone = Instantiate(this);
        clone.amount.SetMinZero();
        return clone;
    } 
}
