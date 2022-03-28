using UnityEngine;

namespace Core.Galaxy
{
    public class FarControl : MonoBehaviour
    {
        Camera camera;
        private void Start()
        {
            camera = GetComponent<Camera>();
        }

        void Update()
        {
            camera.farClipPlane = 3000 * (transform.position.y / 300);
            camera.farClipPlane = Mathf.Clamp(camera.farClipPlane, 500, 3000);

        }
    }
}
