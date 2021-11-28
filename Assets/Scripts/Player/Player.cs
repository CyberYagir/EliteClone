using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipVariables
{
    public float ZRotSpeed, XRotSpeed, YRotSpeed;
    public float maxSpeedUnits, speedUpMultiplier;
}
[System.Serializable]
public class ShipClaped
{
    public float value, max;
}
[System.Serializable]
public class SpaceShip
{
    public Mesh shipModel;
    public ShipClaped fuel, hp, shields;
    public ShipVariables data;
}

public class Player : MonoBehaviour
{
    public static Player inst { get; private set; }
    public ShipController control { get; private set; }
    public WarpManager warp { get; private set; }
    public SaveLoadData saves { get; private set; }

    [SerializeField] SpaceShip spaceShip;

    public static event System.Action OnSceneChanged = delegate {  };

    Cargo cargo;
    TargetManager targets;

    private void Awake()
    {
        Init();
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
        GalaxyGenerator.LoadSystems();
        control = GetComponent<ShipController>();
        cargo = GetComponent<Cargo>();
        targets = GetComponent<TargetManager>();
        warp = GetComponent<WarpManager>();
        saves = GetComponent<SaveLoadData>();
    }

    public SpaceShip Ship()
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
