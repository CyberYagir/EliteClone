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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        headView = Input.GetKey(KeyCode.Mouse2);
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

    public void ForwardBackward()
    {
        if (moveMode == MoveMode.S)
        {
            speed = 0;
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                moveMode = MoveMode.F;
            }
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                moveMode = MoveMode.B;
            }
        }

        speed += Input.GetAxis("Vertical") * Time.deltaTime * player.Ship().data.speedUpMultiplier;
        if (moveMode == MoveMode.F)
            speed = Mathf.Clamp(speed, 0, player.Ship().data.maxSpeedUnits);
        else if (moveMode == MoveMode.B)
            speed = Mathf.Clamp(speed, -player.Ship().data.maxSpeedUnits / 2f, 0);

        if (speed == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            moveMode = MoveMode.S;
        }
        rb.velocity = transform.forward * speed;
    }
}
