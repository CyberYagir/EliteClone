using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSpawnShip : MonoBehaviour
{
    private GameObject ship;
    public void SpawnShip()
    {
        ship = Instantiate(DeathDataCollector.Instance.playerData.Ship.GetShip().shipModel);
    }

    private void Update()
    {
        ship.transform.Rotate(Vector3.up * 5 * Time.deltaTime);
    }
}
