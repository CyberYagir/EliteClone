using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHologram : MonoBehaviour
{
    Quaternion startRotation;


    private void Start()
    {
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        var ship = ShipController.instance;
        transform.localRotation = Quaternion.Lerp(transform.localRotation,  startRotation * Quaternion.Euler(-ship.vertical * ship.player.spaceShip.shipVariables.XRotSpeed, ship.yaw * ship.player.spaceShip.shipVariables.YRotSpeed * ship.player.spaceShip.shipVariables.YRotSpeed, -ship.horizontal * ship.player.spaceShip.shipVariables.ZRotSpeed), 10 * Time.deltaTime);
    }
}
