using UnityEngine;

namespace Core.Demo
{
    public class DemoModeDistance : MonoBehaviour
    {
        [SerializeField] private float distance;
        [SerializeField] private Transform player;
        [SerializeField] private GameObject oldCamera;
        [SerializeField] private GameObject followCamera;
        [SerializeField] private Transform forward;
        private void Update()
        {
            if (Vector3.Distance(transform.position, player.transform.position) > distance)
            {
                oldCamera.GetComponent<Camera>().enabled = false;
                followCamera.SetActive(true);
                forward.transform.rotation = followCamera.transform.rotation;
                forward.localEulerAngles = new Vector3(0, forward.localEulerAngles.y, 0);
                Destroy(this);
            }
        }
    }
}
