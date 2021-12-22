using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "", menuName = "Game/WeaponItem", order = 1)]
public class WeaponItem : Item
{

    public WeaponItem Clone()
    {
        return Instantiate(this);
    }
}
