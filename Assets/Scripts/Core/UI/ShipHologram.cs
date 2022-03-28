using Core.Game;
using Core.Location;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class ShipHologram : MonoBehaviour
    {
        private Quaternion startRotation;
        private Player.Player player;

        public static ShipHologram Instance;
    
        [SerializeField] private Transform floor;
        [SerializeField] private MeshFilter shipModel;
        [SerializeField] private Material material;
        [SerializeField] private Image shieldIndicator;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            startRotation = transform.localRotation;
            player = Player.Player.inst;
            shipModel.sharedMesh = player.Ship().shipModel.GetComponent<MeshFilter>().sharedMesh;

            var matsCount = new Material[10];
            for (int i = 0; i < matsCount.Length; i++)
            {
                matsCount[i] = material;
            }

            shipModel.GetComponent<Renderer>().materials = matsCount;
        }

        private void Update()
        {
            var shields = player.Ship().GetValue(ItemShip.ShipValuesTypes.Shields);
            shieldIndicator.fillAmount = Mathf.Lerp(shieldIndicator.fillAmount, shields.value / shields.max, Time.deltaTime);
            var ship = player.control;
            shipModel.transform.localRotation = Quaternion.Lerp(shipModel.transform.localRotation,  startRotation * Quaternion.Euler(-ship.vertical * player.Ship().data.XRotSpeed, ship.yaw * ship.player.Ship().data.YRotSpeed * player.Ship().data.YRotSpeed, -ship.horizontal * player.Ship().data.ZRotSpeed), 10 * Time.deltaTime);
            LandHologram();
        }

        public void LandHologram()
        {
            if (World.Scene == Scenes.Location)
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
}
