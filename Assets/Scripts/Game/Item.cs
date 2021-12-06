using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class ValueLimit
{
    [SerializeField] float value;
    [SerializeField] float minValue, maxValue;

    public float Value => value;

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

[CreateAssetMenu(fileName = "", menuName = "Game/Item", order = 1)]
public class Item : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] private string name;
    [SerializeField] private Sprite icon;
    [SerializeField] private ValueLimit amount;

    public Item Init()
    {
        id = Random.Range(-100000000, 100000000);
        return this;
    }

    public Item Clone()
    {
        return Instantiate(this).Init();
    }
}
