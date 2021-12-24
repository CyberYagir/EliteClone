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

    [SerializeField] ItemShip spaceShip;

    public static event System.Action OnSceneChanged = delegate {  };

    private Cargo cargo;
    private TargetManager targets;
    
    private float heatTime;
    private void Awake()
    {
        Init();
    }

    public void AddHeat(float heat)
    {
        spaceShip.heat.value += heat * Time.deltaTime;
        if (spaceShip.heat.value > spaceShip.heat.max)
        {
            spaceShip.heat.value = spaceShip.heat.max;
            spaceShip.hp.value -= Time.deltaTime;
            WarningManager.AddWarning("Heat level critical!", WarningTypes.Damage);
            
        }

        if (heat > 0)
            heatTime = 0;
        if (spaceShip.heat.value < 0)
        {
            spaceShip.heat.value = 0;
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
        inst = this;
        spaceShip = spaceShip.Clone();
        GalaxyGenerator.LoadSystems();
        control = GetComponent<ShipController>();
        cargo = GetComponent<Cargo>();
        targets = GetComponent<TargetManager>();
        warp = GetComponent<WarpManager>();
        saves = GetComponent<SaveLoadData>();
        land = GetComponent<LandManager>();
        quests = GetComponent<AppliedQuests>();
    }

    public ItemShip Ship()
    {
        return spaceShip;
    }
    public List<int> Cargo()
    {
        return cargo.items;
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
