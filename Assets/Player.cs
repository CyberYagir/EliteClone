using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipVariables
{
    public float ZRotSpeed, XRotSpeed, YRotSpeed;
}
[System.Serializable]
public class SpaceShip
{
    public Mesh shipModel;
    public ShipVariables shipVariables;
}

public class Player : MonoBehaviour
{
    public SpaceShip spaceShip;
}
