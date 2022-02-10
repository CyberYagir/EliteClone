using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ShipModels : MonoBehaviour
{
    [SerializeField] private Transform modelsHolder;
    [SerializeField] private GameObject spawnedCabine;
    [SerializeField] private GameObject shipRenderer;


    public void InitShip()
    {
        var ship = Player.inst.Ship();
        if (spawnedCabine != null)
        {
            Destroy(spawnedCabine.gameObject);
            Destroy(shipRenderer.gameObject);
        }
        spawnedCabine = Instantiate(ship.shipCabine.gameObject, modelsHolder);
        shipRenderer = Instantiate(ship.shipModel.gameObject, modelsHolder);
        shipRenderer.GetComponent<ShipMeshManager>().InitSlots(Player.inst.Ship());
    }
}
