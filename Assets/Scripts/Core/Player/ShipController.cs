using Core.Systems;
using UnityEngine;
using static Core.Game.ItemShip.ShipValuesTypes;

namespace Core.Player
{
    public class ShipController : MonoBehaviour
    {
        public float horizontal, vertical, yaw;
        public float axisSence;
        public bool freezeControl, headView;
        public Player player;

        public enum MoveMode { S, F, B }
        public MoveMode moveMode = MoveMode.S;

        private Rigidbody rigidbody;

        public float speed;
        private float collisionCooldown;
        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            player = Player.inst;
        }


        private void OnCollisionEnter(Collision collision)
        {
            player.warp.WarpStop();
            player.Ship().GetValue(Health).value -= ((speed + player.warp.warpSpeed));
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            collisionCooldown += Time.deltaTime;
            headView = InputM.GetPressButton(KAction.HeadView);
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
            if (player.warp.warpSpeed > 200)
            {
                player.TakeDamageHeath((player.warp.warpSpeed/1000f) * 100);
            }
            player.warp.warpSpeed = Mathf.Lerp(player.warp.warpSpeed, 0, 20 * Time.deltaTime);
            if (speed < 0.001f) speed = 0;
            if (player.warp.warpSpeed < 0.001f) player.warp.warpSpeed = 0;
        
            rigidbody.velocity = transform.forward * (speed + player.warp.warpSpeed);
        }
    
        public void CheckMinDistanceToObjects()
        {
            if (World.Scene == Scenes.System)
            {
                if (SolarSystemGenerator.objects != null)
                {
                    foreach (var obj in SolarSystemGenerator.objects)
                    {
                        if (Vector3.Distance(obj.transform.position, transform.position) <
                            obj.transform.localScale.magnitude * 0.9f)
                        {
                            var dir = transform.forward * (InputM.GetAxisRaw(KAction.Vertical) == 0 ? 1 : InputM.GetAxisRaw(KAction.Vertical));
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
                if (InputM.GetAxis(KAction.Vertical) > 0)
                {
                    moveMode = MoveMode.F;
                }
                if (InputM.GetAxis(KAction.Vertical) < 0)
                {
                    moveMode = MoveMode.B;
                }
            }
        }
        public void ForceAndWarpControl()
        {
            if (!player.warp.isWarp)
            {
                speed += InputM.GetAxis(KAction.Vertical) * Time.deltaTime * player.Ship().data.speedUpMultiplier;
            }
            else
            {
                CameraShake.Shake();
                player.warp.warpSpeed += InputM.GetAxis(KAction.Vertical) * Time.deltaTime * player.warp.warpSpeedUp;
                if (World.Scene == Scenes.Location)
                {
                    if (player.warp.warpSpeed > player.warp.maxLocationSpeed)
                    {
                        player.warp.warpSpeed = player.warp.maxLocationSpeed;
                    }
                }
                if (player.warp.warpSpeed < 0)
                {
                    player.warp.WarpStop();
                }
            }
        }

        public void Moving()
        {
            if (moveMode == MoveMode.F)
                speed = Mathf.Clamp(speed, 0, player.Ship().data.maxSpeedUnits);
            else if (moveMode == MoveMode.B)
                speed = Mathf.Clamp(speed, -player.Ship().data.maxSpeedUnits / 2f, 0);

            if (speed == 0 && InputM.GetAxisRaw(KAction.Vertical) == 0)
            {
                moveMode = MoveMode.S;
            }
        }

        public bool IsStoping()
        {
            if (InputM.GetPressButton(KAction.Stop))
            {
                StopPlayer();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckFuel()
        {
            var fuel = Player.inst.Ship().GetValue(Fuel);
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
                Player.inst.Ship().GetValue(Fuel).value -= (Mathf.Abs(speed + player.warp.warpSpeed) / 100f) * Time.deltaTime;

                if (!IsStoping())
                {
                    Moving();
                    var calcSpeed = (World.Scene == Scenes.Location && !player.warp.isWarp ? speed * locationSpeedUp : speed);
                    rigidbody.velocity = transform.forward * (calcSpeed + player.warp.warpSpeed);
                }
            }
        }
    }
}
