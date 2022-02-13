using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class WorldDrop : MonoBehaviour
{
    [SerializeField] private Item item;

    public void Init(Item dropped)
    {
        item = dropped;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (Player.inst.cargo.AddItem(item, true))
            {
                Destroy(gameObject);
            }
        }
    }
}
