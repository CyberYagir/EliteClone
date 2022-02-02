using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GarageShipPoint : MonoBehaviour
{
    [SerializeField] private GameObject spawnedShip;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform door;

    private void Awake()
    {
        GarageDataCollect.OnChangeShip += InitShip;
    }

    public void InitShip()
    {
        StartCoroutine(ShipSpawn());
       
    }


    public IEnumerator ShipSpawn()
    {
        var rotator = GetComponent<GarageShipRotator>();
        rotator.enabled = false;
        door.DOLocalMove(new Vector3(door.localPosition.x, 17.5f, door.localPosition.z), 1f);
        if (spawnedShip != null)
        {
            spawnedShip.gameObject.transform.DOMove(spawnPoint.position, 5);
            spawnedShip.gameObject.transform.DOLocalRotate(Vector3.one, 2);
            yield return new WaitForSeconds(5);
            Destroy(spawnedShip.gameObject);
        }
        spawnedShip = Instantiate(GarageDataCollect.Instance.ship.shipModel, transform);
        spawnedShip.transform.position = spawnPoint.position;
        spawnedShip.transform.localScale = Vector3.one;
        spawnedShip.transform.DOLocalMove(Vector3.zero, 5);
        yield return new WaitForSeconds(5);
        rotator.enabled = true;
        
        door.DOLocalMove(new Vector3(door.localPosition.x, 9.2f, door.localPosition.z), 1f);
        yield return new WaitForSeconds(0.9f);
        CameraShake.Shake(0.2f);
    }
}
