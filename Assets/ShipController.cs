using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static ShipController instance;
    public float horizontal, vertical, yaw;
    public float axisSence;
    public bool freezeControl, headView;
    public Player player;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (!freezeControl && !headView)
        {
            horizontal += Input.GetAxis("Mouse X") * Time.deltaTime * axisSence;
            vertical += Input.GetAxis("Mouse Y") * Time.deltaTime * axisSence;

            horizontal = Mathf.Clamp(horizontal, -1, 1);
            vertical = Mathf.Clamp(vertical, -1, 1);
            yaw = Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        headView = Input.GetKey(KeyCode.Mouse2);

        transform.rotation *= Quaternion.Euler(player.spaceShip.shipVariables.XRotSpeed * -vertical * Time.deltaTime, player.spaceShip.shipVariables.YRotSpeed * yaw, player.spaceShip.shipVariables.ZRotSpeed * -horizontal * Time.deltaTime);
    }
}
