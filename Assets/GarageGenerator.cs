using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class GarageGenerator : MonoBehaviour
{
    public static GarageGenerator Instance;
    
    [SerializeField] private SaveLoadData saves;
    [SerializeField] private ItemShip ship;
    private PlayerData playerData;

    public static Event OnChangeShip = new Event();
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadShip();
    }

    public void LoadShip()
    {
        playerData = saves.LoadData();
        ship = playerData.Ship.GetShip();
        OnChangeShip.Invoke();
    }

    public ItemShip GetShip() => ship;
}
