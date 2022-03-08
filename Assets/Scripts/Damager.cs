using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{
    [HideInInspector]
    public Event OnDamaged;
    [HideInInspector]
    public Event OnDeath;

    public void TakeDamage(ref float health, float damage)
    {
        health -= damage;
    }

}
