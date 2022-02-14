using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using static Game.ItemShip.ShipValuesTypes;

interface IDamagable
{
    public void TakeDamage(float damage);
}

public class Player : MonoBehaviour, IDamagable
{
    public static Player inst { get; private set; }
    public ShipController control { get; private set; }
    public WarpManager warp { get; private set; }
    public SaveLoadData saves { get; private set; }
    public LandManager land { get; private set; }
    public AppliedQuests quests { get; private set; }
    public Cargo cargo { get; private set; }
    public ShipAttack attack { get; private set; }

    public Damager damager { get; private set; }


    public static Event OnSceneChanged = new Event();

    private TargetManager targets;
    private ShipModels models;
    Ship spaceShip;
    
    private float heatTime;
    private void Awake()
    {
        OnSceneChanged = new Event();
        Init();
    }

    public void AddHeat(float heat)
    {
        var currShip = spaceShip.GetShip();
        var temperature = currShip.GetValue(Temperature);
        temperature.value += heat * Time.deltaTime;
        if (temperature.value > temperature.max)
        {
            temperature.Clamp();
            TakeDamage(Time.deltaTime);
            WarningManager.AddWarning("Heat level critical!", WarningTypes.Damage);
        }

        if (heat > 0)
            heatTime = 0;
        if (currShip.GetValue(Temperature).value < 0)
        {
            currShip.GetValue(Temperature).value = 0;
        }
    }
    
    public void StopAxis()
    {
        control.horizontal = 0;
        control.vertical = 0;
        control.speed = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    
    private void Update()
    {
        DownHeat();
    }

    public void DownHeat()
    {
        heatTime += Time.deltaTime;
        if (heatTime > 2)
        {
            AddHeat(-10);
        }
    }

    private void Start()
    {
        ChangeScene();
    }

    public static void ChangeScene()
    {
        OnSceneChanged.Run(); 
    }

    public void HardStop()
    {
        control.speed = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    public void Init()
    {
        if (inst == null)
        {
            inst = this;
            spaceShip = GetComponent<Ship>();
            GalaxyGenerator.LoadSystems();
            control = GetComponent<ShipController>();
            cargo = GetComponent<Cargo>();
            targets = GetComponent<TargetManager>();
            warp = GetComponent<WarpManager>();
            saves = GetComponent<SaveLoadData>();
            land = GetComponent<LandManager>();
            quests = GetComponent<AppliedQuests>();
            models = GetComponent<ShipModels>();
            attack = GetComponent<ShipAttack>();
            damager = GetComponent<Damager>();

            spaceShip.OnChangeShip += models.InitShip;
            spaceShip.SetShip(spaceShip.CloneShip());
        }
    }

    public ItemShip Ship()
    {
        return spaceShip.GetShip();
    }
    
    public void LoadShip(ShipData data)
    {
        spaceShip.LoadShip(data);
        spaceShip.GetShip().ValuesToDictionary();
    }
    
    
    public WorldSpaceObject GetTarget()
    {
        return targets.target;
    }
    public void SetTarget(WorldSpaceObject target)
    {
        targets.SetTarget(target);
    }
    public List<ContactObject> GetContacts()
    {
        return targets.contacts;
    }

    public void TakeDamage(float damage)
    {
        damager.TakeDamage(ref spaceShip.GetShip().GetValue(Health).value, damage);
    }
}
