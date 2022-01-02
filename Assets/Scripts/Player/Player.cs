using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;


[System.Serializable]
public class SpaceShip
{
    
}

public class Player : MonoBehaviour
{
    public static Player inst { get; private set; }
    public ShipController control { get; private set; }
    public WarpManager warp { get; private set; }
    public SaveLoadData saves { get; private set; }
    public LandManager land { get; private set; }
    public AppliedQuests quests { get; private set; }
    public Cargo cargo { get; private set; }
    
    
    

    public static event System.Action OnSceneChanged = delegate {  };

    private TargetManager targets;
    private ShipModels models;
    Ship spaceShip;
    
    private float heatTime;
    private void Awake()
    {
        OnSceneChanged = delegate {  };
        Init();
    }

    public void AddHeat(float heat)
    {
        var currShip = this.spaceShip.GetShip();
        currShip.heat.value += heat * Time.deltaTime;
        if (currShip.heat.value > currShip.heat.max)
        {
            currShip.heat.value = currShip.heat.max;
            currShip.hp.value -= Time.deltaTime;
            WarningManager.AddWarning("Heat level critical!", WarningTypes.Damage);
        }

        if (heat > 0)
            heatTime = 0;
        if (currShip.heat.value < 0)
        {
            currShip.heat.value = 0;
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
        OnSceneChanged(); 
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
            spaceShip.SetShip(spaceShip.CloneShip());
            GalaxyGenerator.LoadSystems();
            control = GetComponent<ShipController>();
            cargo = GetComponent<Cargo>();
            targets = GetComponent<TargetManager>();
            warp = GetComponent<WarpManager>();
            saves = GetComponent<SaveLoadData>();
            land = GetComponent<LandManager>();
            quests = GetComponent<AppliedQuests>();
            models = GetComponent<ShipModels>();
            models.InitShip(Ship());
        }
    }

    public ItemShip Ship()
    {
        return spaceShip.GetShip();
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
}
