using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] FreeCam freeCam;
    private void Update()
    {
        freeCam.enabled = Input.GetKey(KeyCode.Mouse1);
        Cursor.lockState = freeCam.enabled ? CursorLockMode.Locked : CursorLockMode.Confined;
        Cursor.visible = !freeCam.enabled;
    }
}
