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

    [SerializeField] SpaceShip spaceShip;

    Cargo cargo;
    TargetManager targets;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        inst = this;
        control = GetComponent<ShipController>();
        cargo = GetComponent<Cargo>();
        targets = GetComponent<TargetManager>();
        warp = GetComponent<WarpManager>();
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
