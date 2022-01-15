using System.Collections;
using System.Collections.Generic;
using Game;
using Newtonsoft.Json;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private ItemShip ship;
    public Event OnChangeShip = new Event();
    
    
    public void SetShip(ItemShip item)
    {
        ship = item;
        OnChangeShip.Run();
    }
    public ItemShip GetShip()
    {
        return ship;
    }

    public ItemShip CloneShip()
    {
        return ship.Clone(); 
    }

    public void LoadShip(ShipData data)
    {
        SetShip(data.GetShip());
    }
}
