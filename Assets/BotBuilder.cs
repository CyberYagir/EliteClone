using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotBuilder : MonoBehaviour, IDamagable
{
    public int uniqID = -1;
    [SerializeField] private BotAttackController attackControl;
    [SerializeField] private BotLandController landControl;
    [SerializeField] private BotMovingController movingControl;
    [SerializeField] private ShieldActivator shield;
    [SerializeField] private BotVisual visual;
    [SerializeField] private ContactObject contactManager;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Ship ship;
    [SerializeField] private GameObject explodePrefab, dropPrefab;
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
        ship.SetShip(shipData);
        SetName();
    }
    
    public void InitBot(System.Random rnd = null)
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

        transform.name = GetVisual().GetShipName() + $" [{firstName} {lastName}]";
    }

    public void SetName()
    {
        transform.name = GetVisual().GetShipName() + " [" + transform.name.Split('[')[1];
    }
    public void InitBot(string first, string last)
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
        Attack, Land, Moving, Stationary
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
        var ship = this.ship.GetShip();
        if (ship.GetValue(ItemShip.ShipValuesTypes.Shields).value <= 0)
        {
            damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Health).value, damage);
            shield.isActive = false;
            if (ship.GetValue(ItemShip.ShipValuesTypes.Health).value <= 0)
            {
                Death();
            }
        }
        else
        {
            damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Shields).value, damage);
            shield.isActive = true;
        }
    }

    public void Death()
    {
        if (uniqID != -1)
        {
            SolarSystemShips.Instance.AddDead(this);
        }

        Drop();
        
        Destroy(Instantiate(explodePrefab, transform.position, transform.rotation), 120);
        Destroy(gameObject);
    }

    public void Drop()
    {
        var rnd = new System.Random(uniqID);
        if (uniqID == -1)
        {
            rnd = new System.Random();
        }
        var dropCount = rnd.Next(0, 6);
        for (int i = 0; i < dropCount; i++)
        {
            var drop = Instantiate(dropPrefab, transform.position, transform.rotation).GetComponent<WorldDrop>();
            Physics.IgnoreCollision(drop.GetComponent<BoxCollider>(), Player.inst.GetComponentInChildren<Collider>(), true);
            drop.Init(ItemsManager.GetRewardItem(rnd));
            drop.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere, ForceMode.Impulse);
        }   
    }
    
    public ItemShip GetShip()
    {
        return ship.GetShip();
    }
}
