using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WorldOrbitalStation : MonoBehaviour
{
    [SerializeField] private int uniqSeed;
    public Transform spawnPoint;
    private void Start()
    {
        uniqSeed = 0;
        foreach (var ch in Encoding.ASCII.GetBytes(transform.name))
        {
            uniqSeed += ch;
        }
        uniqSeed *= uniqSeed;
    }
}
