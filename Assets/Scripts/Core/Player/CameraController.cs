using Core.Galaxy;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private FreeCam freeCam;
        private void Update()
        {
            if (!PlayerDataManager.Instance.RunHandler.IsPaused)
            {
                freeCam.enabled = InputService.GetAxisIsActive(KAction.Click);
                Cursor.lockState = freeCam.enabled ? CursorLockMode.Locked : CursorLockMode.Confined;
                Cursor.visible = !freeCam.enabled;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }
}
