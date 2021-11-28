using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHologram : MonoBehaviour
{
    Quaternion startRotation;
    Player player;

    [SerializeField] private Transform floor;
    [SerializeField] private MeshFilter shipModel;

    private void Start()
    {
        startRotation = transform.localRotation;
        player = Player.inst;
        shipModel.sharedMesh = player.Ship().shipModel;
    }

    private void Update()
    {
        var ship = player.control;
        shipModel.transform.localRotation = Quaternion.Lerp(shipModel.transform.localRotation,  startRotation * Quaternion.Euler(-ship.vertical * player.Ship().data.XRotSpeed, ship.yaw * ship.player.Ship().data.YRotSpeed * player.Ship().data.YRotSpeed, -ship.horizontal * player.Ship().data.ZRotSpeed), 10 * Time.deltaTime);

        if (Application.loadedLevelName == "Location")
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 10))
            {
                floor.gameObject.SetActive(hit.transform.GetComponent<LandPoint>());
                if (floor.gameObject.active)
                {
                    floor.transform.localPosition = player.transform.InverseTransformPoint(hit.transform.position) *
                                                    transform.localScale.magnitude;
                    floor.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
            }
            else
            {
                floor.gameObject.SetActive(false);
            }
        }
        else
        {
            floor.gameObject.SetActive(false);
        }
    }
}
