using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private ItemShip ship;

    
    
    public void SetShip(ItemShip item)
    {
        ship = item;
    }
    public ItemShip GetShip()
    {
        return ship;
    }

    public ItemShip CloneShip()
    {
        return ship.Clone();
    }
}
