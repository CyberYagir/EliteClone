using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHologram : MonoBehaviour
{
    Quaternion startRotation;
    Player player;

    private void Start()
    {
        startRotation = transform.localRotation;
        player = Player.inst;
        GetComponent<MeshFilter>().sharedMesh = player.Ship().shipModel;
    }

    private void Update()
    {
        var ship = player.control;
        transform.localRotation = Quaternion.Lerp(transform.localRotation,  startRotation * Quaternion.Euler(-ship.vertical * player.Ship().data.XRotSpeed, ship.yaw * ship.player.Ship().data.YRotSpeed * player.Ship().data.YRotSpeed, -ship.horizontal * player.Ship().data.ZRotSpeed), 10 * Time.deltaTime);
    }
}
