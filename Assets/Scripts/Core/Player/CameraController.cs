using Core.Galaxy;
using UnityEngine;

namespace Core.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private FreeCam freeCam;
        private void Update()
        {
            freeCam.enabled = InputM.GetPressButton(KAction.Click);
            Cursor.lockState = freeCam.enabled ? CursorLockMode.Locked : CursorLockMode.Confined;
            Cursor.visible = !freeCam.enabled;
        }
    }
}
