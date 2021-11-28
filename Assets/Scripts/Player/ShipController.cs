using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float horizontal, vertical, yaw;
    public float axisSence;
    public bool freezeControl, headView;
    public Player player;

    public enum MoveMode { S, F, B }
    public MoveMode moveMode = MoveMode.S;

    Rigidbody rb;

    public float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = Player.inst;
    }


    private void OnCollisionEnter(Collision collision)
    {
        player.warp.WarpStop();
        player.Ship().hp.value -= ((speed + player.warp.warpSpeed));
        speed *= -1;
        WarningManager.AddWarning("Damage when touched by the mesh.", WarningTypes.Damage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
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
        player.warp.warpSpeed = Mathf.Lerp(player.warp.warpSpeed, 0, 20 * Time.deltaTime);
        if (speed < 0.001f) speed = 0;
        if (player.warp.warpSpeed < 0.001f) player.warp.warpSpeed = 0;
        
        rb.velocity = transform.forward * (speed + player.warp.warpSpeed);
    }
    
    public void ForwardBackward()
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

        if (SolarSystemGenerator.objects != null)
        {
            foreach (var obj in SolarSystemGenerator.objects)
            {
                if (Vector3.Distance(obj.transform.position, transform.position) <
                    obj.transform.localScale.magnitude * 0.9f)
                {
                    print(Vector3.Distance(obj.transform.position, transform.position) + " / " + obj.transform.localScale.magnitude * 0.9f);
                    
                    if (Physics.Raycast(transform.position, transform.forward * (InputM.GetAxisRaw(KAction.Vertical) == 0 ? 1 : InputM.GetAxisRaw(KAction.Vertical)), out RaycastHit hit))
                    {
                        player.HardStop();
                    }
                    
                    if (obj.transform.tag == "Sun")
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

        
        
        if (Player.inst.Ship().fuel.value <= 0) { return; }

        if (InputM.GetAxisUp(KAction.Vertical)) return;

        
        
        
        Player.inst.Ship().fuel.value -= (Mathf.Abs(speed + player.warp.warpSpeed)/100f) * Time.deltaTime;
        if (InputM.GetPressButton(KAction.Stop))
        {
            StopPlayer();
            return;
        }
        
        if (!player.warp.isWarp)
        {
            speed += InputM.GetAxis(KAction.Vertical) * Time.deltaTime * player.Ship().data.speedUpMultiplier; 
        }
        else
        {
            CameraShake.Shake();
            player.warp.warpSpeed += InputM.GetAxis(KAction.Vertical) * Time.deltaTime * player.warp.warpSpeedUp;
            if (player.warp.warpSpeed < 0)
            {
                player.warp.WarpStop();
            }
        }
        
        if (moveMode == MoveMode.F)
            speed = Mathf.Clamp(speed, 0, player.Ship().data.maxSpeedUnits);
        else if (moveMode == MoveMode.B)
            speed = Mathf.Clamp(speed, -player.Ship().data.maxSpeedUnits / 2f, 0);

        if (speed == 0 && InputM.GetAxisRaw(KAction.Vertical) == 0)
        {
            moveMode = MoveMode.S;
        }
        rb.velocity = transform.forward * (speed + player.warp.warpSpeed);
    }
}
