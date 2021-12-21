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

    public void SetClamp(float min, float max)
    {
        minValue = min;
        maxValue = max;
        if (minValue > maxValue)
        {
            maxValue = minValue;
        }
    }
    public void SetValue(float val)
    {
        value = val;
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
[CreateAssetMenu(fileName = "", menuName = "Game/Item", order = 1)]
public class Item : ScriptableObject
{
    public IDTruple id;
    public string itemName;
    public Sprite icon;
    public ValueLimit amount;

    public Item Clone()
    {
        return Instantiate(this);
    }
}
