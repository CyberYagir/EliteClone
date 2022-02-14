using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        if (other.GetComponent<ShipMeshManager>())
        {
            if (Player.inst.cargo.AddItem(item, true))
            {
                transform.DOMove(other.transform.position, 0.7f);
                transform.DOScale(Vector3.zero, 0.4f);
                foreach (var col in GetComponents<Collider>())
                {
                    col.enabled = false;
                }
                Destroy(gameObject,1);
                Destroy(this);
            }
        }
    }
}
