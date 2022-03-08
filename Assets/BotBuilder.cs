using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotBuilder : MonoBehaviour, IDamagable
{
    [SerializeField] private BotAttackController attackControl;
    [SerializeField] private BotLandController landControl;
    [SerializeField] private BotMovingController movingControl;
    [SerializeField] private ShieldActivator shield;
    [SerializeField] private BotVisual visual;
    [SerializeField] private ContactObject contactManager;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private ItemShip ship;
    private Damager damager;
    private void Start()
    {
        damager = GetComponent<Damager>();
        if (World.Scene == Scenes.Location)
        {
            attackControl.enabled = false;
            landControl.enabled = true;
        }
        else
        {
            attackControl.enabled = true;
            landControl.enabled = false;
        }
        
    }

    public void SetShip(ItemShip shipData)
    {
        ship = shipData;
    }
    
    public void InitBot(bool triggerEvent, System.Random rnd = null)
    {
        NamesHolder.Init();
        var firstName = "";
        var lastName = "";
        if (rnd == null)
        {
            firstName = NamesHolder.ToUpperFist(NamesHolder.Instance.FirstNames[UnityEngine.Random.Range(0, NamesHolder.Instance.FirstNames.Length)]);
            lastName = NamesHolder.ToUpperFist(NamesHolder.Instance.LastNames[UnityEngine.Random.Range(0, NamesHolder.Instance.LastNames.Length)]);
        }
        else
        {
            firstName = NamesHolder.ToUpperFist(NamesHolder.GetFirstName(rnd));
            lastName = NamesHolder.ToUpperFist(NamesHolder.GetLastName(rnd));
        }

        transform.name = GetComponent<BotVisual>().GetShipName() + $" [{firstName} {lastName}]";
    }
    public void InitBot(bool triggerEvent, string first, string last)
    {
        NamesHolder.Init();
        transform.name = GetComponent<BotVisual>().GetShipName() + $" [{first} {last}]";
    }
    public ParticleSystem PlayWarp()
    {
        particles.Play();
        return particles;
    }

    public enum BotState
    {
        Attack, Land, Moving
    }

    public void SetBehaviour(BotState botState)
    {
        attackControl.enabled = botState == BotState.Attack;
        landControl.enabled = botState == BotState.Land;
        movingControl.enabled = botState == BotState.Moving;
    }
    
    public ShieldActivator GetShield()
    {
        return shield;
    }

    public BotVisual GetVisual()
    {
        return visual;
    }

    public void AddContact(bool trigger)
    {
        contactManager.Init(trigger);
    }

    public void TakeDamage(float damage)
    {
        SetBehaviour(BotState.Attack);
        attackControl.SetTarget(Player.inst.transform);
        if (ship.GetValue(ItemShip.ShipValuesTypes.Shields).value <= 0)
        {
            damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Health).value, damage);
            shield.isActive = false;
        }
        else
        {
            damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Shields).value, damage);
            shield.isActive = true;
        }
    }

    public ItemShip GetShip()
    {
        return ship;
    }
}
