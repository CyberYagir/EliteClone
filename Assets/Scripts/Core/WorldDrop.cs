using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;

public class WorldDrop : MonoBehaviour
{
    [SerializeField] private Item item;
    private float cooldown = 0;
    public void Init(Item dropped, float delay = 0)
    {
        item = dropped;
        cooldown = delay;
        GetComponent<BoxCollider>().enabled = true;
        transform.name = "Storage: " + item.itemName + $" [{item.amount.value}]";
        GetComponent<ContactObject>().Init(true);
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cooldown <= 0)
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

                    Destroy(gameObject, 1);
                    Destroy(this);
                }
            }
        }
    }
}
