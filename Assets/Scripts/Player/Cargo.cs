using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    public List<int> items { get; private set; } =  new List<int>();

    private void Awake()
    {
    }
}
