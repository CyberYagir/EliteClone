using Core.Systems;
using UnityEngine;
using static Core.Game.ItemShip.ShipValuesTypes;

namespace Core.PlayerScripts
{
    public class ShipController : MonoBehaviour
    {
        public enum MoveMode
        {
            S,
            F,
            B
        }

        
        
        public float horizontal { get; private set; }
        public float vertical { get; private set; }
        public float yaw { get; private set; }
        public float axisSence { get; private set; } = 5;
        public bool headView { get; private set; }
        public MoveMode moveMode { get; private set; } = MoveMode.S;
        public float speed { get; private set; }
        public Camera Camera => mainCamera;

        [SerializeField] private Camera mainCamera;

        private bool freezeControl;
        private Player player;
        private Rigidbody rigidbody;
        private float collisionCooldown;

        public WarpManager warp => player.WarpManager;
        public float speedPercent => speed / player.Ship().data.maxSpeedUnits;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            player = GetComponent<Player>();
        }


        private void OnCollisionEnter(Collision collision)
        {
            player.WarpManager.WarpStop();
            player.Ship().GetValue(Health).value -= ((speed + player.WarpManager.warpSpeed));
            if (collisionCooldown > 2f)
            {
                speed = player.Ship().data.maxSpeedUnits * 0.05f;
                moveMode = MoveMode.B;
            }

            player.TakeDamageHeath(0);
            WarningManager.AddWarning("Damage when touched by the mesh.", WarningTypes.Damage);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.timeScale != 0)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            collisionCooldown += Time.deltaTime;
            headView = InputService.GetAxisIsActive(KAction.HeadView);
            RotationControl();
            ForwardBackward();
        }

        public void RotationControl()
        {
            if (!freezeControl && !headView)
            {
                horizontal += Input.GetAxis("Mouse X") * Time.deltaTime * axisSence;
                vertical += Input.GetAxis("Mouse Y") * Time.deltaTime * axisSence;

                if (new Vector2(vertical, horizontal).magnitude < 0.1f && Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0)
                {
                    vertical = 0;
                    horizontal = 0;
                }

                horizontal = Mathf.Clamp(horizontal, -1, 1);
                vertical = Mathf.Clamp(vertical, -1, 1);
                yaw = Input.GetAxis("Horizontal") * Time.deltaTime;

            }

            transform.rotation *= Quaternion.Euler(player.Ship().data.XRotSpeed * -vertical * Time.deltaTime, player.Ship().data.YRotSpeed * yaw, player.Ship().data.ZRotSpeed * -horizontal * Time.deltaTime);
        }

        public void StopPlayer()
        {
            speed = Mathf.Lerp(speed, 0, 20 * Time.deltaTime);
            if (player.WarpManager.warpSpeed > 200)
            {
                player.TakeDamageHeath((player.WarpManager.warpSpeed / 1000f) * 100);
            }

            player.WarpManager.warpSpeed = Mathf.Lerp(player.WarpManager.warpSpeed, 0, 20 * Time.deltaTime);
            if (speed < 0.001f) speed = 0;
            if (player.WarpManager.warpSpeed < 0.001f) player.WarpManager.warpSpeed = 0;

            rigidbody.velocity = transform.forward * (speed + player.WarpManager.warpSpeed);
        }

        public void CheckMinDistanceToObjects()
        {
            if (World.Scene == Scenes.System)
            {
                if (SolarStaticBuilder.Objects != null)
                {
                    foreach (var obj in SolarStaticBuilder.Objects)
                    {
                        if (Vector3.Distance(obj.transform.position, transform.position) <
                            obj.transform.localScale.magnitude * 0.9f)
                        {
                            var dir = transform.forward * (InputService.GetAxisRaw(KAction.Vertical) == 0 ? 1 : InputService.GetAxisRaw(KAction.Vertical));
                            if (Physics.Raycast(transform.position, dir, float.MaxValue, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
                            {
                                player.HardStop();
                            }

                            if (obj.transform.CompareTag("Sun"))
                            {
                                WarningManager.AddWarning("The cosmic body in front of you is too hot. Don't get any closer.", WarningTypes.Heat);
                                player.AddHeat(10);
                            }
                            else
                            {
                                WarningManager.AddWarning("Too close to the planet's atmosphere.", WarningTypes.Heat);
                            }
                        }
                    }
                }
            }
        }

        public void ForwardBackwardMove()
        {
            if (moveMode == MoveMode.S)
            {
                speed = 0;
                if (InputService.GetAxis(KAction.Vertical) > 0)
                {
                    moveMode = MoveMode.F;
                }

                if (InputService.GetAxis(KAction.Vertical) < 0)
                {
                    moveMode = MoveMode.B;
                }
            }
        }

        public void ForceAndWarpControl()
        {
            if (!player.WarpManager.isWarp)
            {
                speed += InputService.GetAxis(KAction.Vertical) * Time.deltaTime * player.Ship().data.speedUpMultiplier;
            }
            else
            {
                CameraShake.Shake();
                player.WarpManager.warpSpeed += InputService.GetAxis(KAction.Vertical) * Time.deltaTime * player.WarpManager.warpSpeedUp;
                if (World.Scene == Scenes.Location)
                {
                    if (player.WarpManager.warpSpeed > player.WarpManager.maxLocationSpeed)
                    {
                        player.WarpManager.warpSpeed = player.WarpManager.maxLocationSpeed;
                    }
                }

                if (player.WarpManager.warpSpeed < 0)
                {
                    player.WarpManager.WarpStop();
                }
            }
        }

        public void Moving()
        {
            if (moveMode == MoveMode.F)
                speed = Mathf.Clamp(speed, 0, player.Ship().data.maxSpeedUnits);
            else if (moveMode == MoveMode.B)
                speed = Mathf.Clamp(speed, -player.Ship().data.maxSpeedUnits / 2f, 0);

            if (speed == 0 && InputService.GetAxisRaw(KAction.Vertical) == 0)
            {
                moveMode = MoveMode.S;
            }
        }

        public bool IsStoping()
        {
            if (InputService.GetAxisIsActive(KAction.Stop))
            {
                StopPlayer();
                return true;
            }

            return false;
        }

        public bool CheckFuel()
        {
            var fuel = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Fuel);
            if (fuel.value < fuel.max * 0.3f)
            {
                WarningManager.AddWarning("Not enough fuel in the tank.", WarningTypes.Fuel);
            }

            if (fuel.value <= 0)
            {
                return false;
            }

            return true;
        }

        private const float locationSpeedUp = 5;

        public void ForwardBackward()
        {
            ForwardBackwardMove();
            CheckMinDistanceToObjects();
            ForceAndWarpControl();
            if (CheckFuel())
            {
                PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(Fuel).value -= (Mathf.Abs(speed + player.WarpManager.warpSpeed) / 100f) * Time.deltaTime;

                if (!IsStoping())
                {
                    Moving();
                    var calcSpeed = (World.Scene == Scenes.Location && !player.WarpManager.isWarp ? speed * locationSpeedUp : speed);
                    rigidbody.velocity = transform.forward * (calcSpeed + player.WarpManager.warpSpeed);
                }
            }
        }

        public void SetZero()
        {
            horizontal = 0;
            vertical = 0;
            speed = 0;
        }

        public void HardStop()
        {
            speed = 0;
            rigidbody.velocity = Vector3.zero;
        }
    }
}
