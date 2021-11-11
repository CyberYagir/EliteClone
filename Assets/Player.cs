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
public class SpaceShip
{
    public Mesh shipModel;
    public ShipVariables data;
}

public class Player : MonoBehaviour
{
    public static Player inst { get; private set; }
    [SerializeField] SpaceShip spaceShip;
    public ShipController control { get; private set; }

    private void Awake()
    {
        inst = this;
        control = GetComponent<ShipController>();
    }

    public SpaceShip Ship()
    {
        return spaceShip;
    }
}
