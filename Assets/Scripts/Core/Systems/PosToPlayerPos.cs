using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosToPlayerPos : MonoBehaviour
{
    public void Update()
    {
        transform.position = Player.inst.transform.position;
    }
}