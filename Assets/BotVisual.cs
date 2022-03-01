using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BotVisual : MonoBehaviour
{
    public List<GameObject> ships = new List<GameObject>();
    private List<GameObject> engines = new List<GameObject>();

    private void Awake()
    {
        foreach (var lights in GetComponentsInChildren<Light>(true))
        {
            engines.Add(lights.transform.parent.gameObject);
        }
    }

    public void SetVisual(Random rnd)
    {
        ships[rnd.Next(0, ships.Count)].SetActive(true);
    }

    public void ActiveLights()
    {
        foreach (var lights in engines)
        {
            lights.SetActive(true);
        }
    }

    public void SetLights(bool state)
    {
        var current = ships.Find(x => x.active);
        if (current != null)
        {
            foreach (var lights in engines)
            {
                lights.gameObject.SetActive(state);
            }
        }
    }
}