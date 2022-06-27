using Core.Galaxy;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private FreeCam freeCam;
        private void Update()
        {
            freeCam.enabled = InputM.GetAxisIsActive(KAction.Click);
            Cursor.lockState = freeCam.enabled ? CursorLockMode.Locked : CursorLockMode.Confined;
            Cursor.visible = !freeCam.enabled;
        }
    }
}
