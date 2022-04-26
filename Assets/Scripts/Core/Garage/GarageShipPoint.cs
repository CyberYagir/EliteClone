using System.Collections;
using Core.Game;
using Core.PlayerScripts;
using DG.Tweening;
using UnityEngine;

namespace Core.Garage
{
    public class GarageShipPoint : MonoBehaviour
    {
        private ItemShip oldShip;
        
        
        [Header("Inside")]
        [SerializeField] private GameObject spawnedShipInGarage;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform door;
        [Header("Outside")]
        [SerializeField] private GameObject spawnedShipOutside;
        [SerializeField] private GameObject spawnedShipOutsideBack;
        [SerializeField] private GameObject spawnedShipOutsideMiddle;
        [SerializeField] private Renderer shield;
        [SerializeField] private Transform outsidePoint;
        [SerializeField] private Transform wall;
        [SerializeField] private Transform pillars;
        [SerializeField] private Transform car;
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");


        private void Awake()
        {
            GarageDataCollect.OnChangeShip += InitShip;
        }

        public void InitShip()
        {
            StopAllCoroutines();
            
            StartCoroutine(ShipSpawnInGarage());
            StartCoroutine(ShipSpawnOutSide());
        }
        
        private bool isWallOpened = false;

        public IEnumerator ShipSpawnOutSide()
        {
            wall.DOKill();
            yield return new WaitForSeconds(1);
            if (GarageDataCollect.Instance.ship.data.outsideGarage && isWallOpened == false)
            {
                yield return StartCoroutine(OpenShield());
            }

            if (spawnedShipOutside)
            {
                spawnedShipOutside.transform.DOMove(spawnedShipOutsideBack.transform.position, 1f);
                yield return new WaitForSeconds(1f);
                Destroy(spawnedShipOutside.gameObject);
            }

            if (GarageDataCollect.Instance.ship.data.outsideGarage)
            {
                if (isWallOpened == false)
                {
                    yield return StartCoroutine(OpenShield());
                }
                spawnedShipOutside = Instantiate(GarageDataCollect.Instance.ship.shipModel, transform);
                spawnedShipOutside.transform.localScale = Vector3.one;
                spawnedShipOutside.transform.position = outsidePoint.position;
                spawnedShipOutside.transform.DOMove(spawnedShipOutsideMiddle.transform.position, 1f);
                spawnedShipOutside.transform.parent = null;
            }
            else
            {
                wall.DORotate(new Vector3(0, 0, 0), 2f);
                pillars.DOLocalMoveY(-1.62f, 0.5f);
                car.transform.DOLocalMove(new Vector3(3.81f, -1.2f, 14.62f), 1f);
                StartCoroutine(RemoveOpacityShield());
                yield return new WaitForSeconds(2f);
                isWallOpened = false;
            }
            
        }

        public IEnumerator OpenShield()
        {
            wall.DORotate(new Vector3(0, 0, -175), 5f);
            pillars.DOLocalMoveY(-15, 0.5f);
            car.transform.DOLocalMove(new Vector3(8.81f, -1.2f, 18.6f), 1f);
            StartCoroutine(AddOpacityShield());
            yield return new WaitForSeconds(5f);
            isWallOpened = true;
        }
        
        
        public IEnumerator RemoveOpacityShield()
        {
            var opacity = shield.material.GetFloat(Opacity);
            while (opacity > 0f)
            {
                opacity -= Time.deltaTime;
                shield.material.SetFloat(Opacity, opacity);
                yield return null;
            }
        }
        public IEnumerator AddOpacityShield()
        {
            var opacity = shield.material.GetFloat(Opacity);
            while (opacity < 1f)
            {
                opacity += Time.deltaTime;
                shield.material.SetFloat(Opacity, opacity);
                yield return null;
            }
        }
        
        public IEnumerator ShipSpawnInGarage()
        {
            var rotator = GetComponent<GarageShipRotator>();
            rotator.enabled = false;
            bool isOpenedDoor = false;
            //BackOldShip
            if (spawnedShipInGarage != null)
            {
                yield return StartCoroutine(OpenCloseDoor(17.5f, false));
                isOpenedDoor = true;
                yield return new WaitForSeconds(1f);
                spawnedShipInGarage.gameObject.transform.DOMove(spawnPoint.position, 5);
                yield return new WaitForSeconds(5f);
                Destroy(spawnedShipInGarage.gameObject);
            }
            //Spawn New Ship And Move To
            if (!GarageDataCollect.Instance.ship.data.outsideGarage)
            {
                if (!isOpenedDoor)
                {
                    yield return StartCoroutine(OpenCloseDoor(17.5f, false));
                    isOpenedDoor = true;
                }
                spawnedShipInGarage = SpawnShipGarage();
                yield return new WaitForSeconds(5);
                rotator.enabled = true;
            }
            //Close Door
            if (isOpenedDoor)
            {
                yield return StartCoroutine(OpenCloseDoor(9.2f, true));
            }
        }

        public IEnumerator OpenCloseDoor(float y, bool shake)
        {
            door.DOLocalMove(new Vector3(door.localPosition.x,y, door.localPosition.z), 1f);
            yield return new WaitForSeconds(0.9f);
            if (shake)
            {
                CameraShake.Shake(0.2f);
            }
        }
        
        public GameObject SpawnShipGarage()
        {
            var ship = Instantiate(GarageDataCollect.Instance.ship.shipModel, transform);
            ship.transform.name = GarageDataCollect.Instance.ship.shipName;
            ship.transform.position = spawnPoint.position;
            ship.transform.localScale = Vector3.one;
            ship.transform.DOLocalMove(Vector3.zero, 5);

        
        
            var manager = ship.GetComponent<ShipMeshManager>();
            manager.SetCurrentShip(GarageDataCollect.Instance.ship);
            GarageDataCollect.Instance.ship.OnChangeShipData += manager.SetInitSlotsWithoutArgs;
            GarageDataCollect.Instance.ship.OnChangeShipData.Run();

            return ship.gameObject;
        }
    }
}
