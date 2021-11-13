using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] FreeCam freeCam;
    private void Update()
    {
        freeCam.enabled = InputM.GetButton(KAction.Click);
        Cursor.lockState = freeCam.enabled ? CursorLockMode.Locked : CursorLockMode.Confined;
        Cursor.visible = !freeCam.enabled;
    }
}
