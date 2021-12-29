using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ShipModels : MonoBehaviour
{
    [SerializeField] private Transform modelsHolder;
    [SerializeField] private GameObject spawnedCabine;
    [SerializeField] private MeshFilter shipRenderer;


    public void InitShip(ItemShip ship)
    {
        if (spawnedCabine != null)
        {
            Destroy(spawnedCabine.gameObject);
        }
        spawnedCabine = Instantiate(ship.shipCabine.gameObject, modelsHolder);
        shipRenderer.mesh = ship.shipModel;
    }
}
