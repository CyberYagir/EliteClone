using UnityEngine;

namespace Core.Init
{
    public class InitCameraAnim : MonoBehaviour
    {
        [SerializeField] private float maxLeft, speed, maxAngle;
        public float percent;

        private void Update()
        {
            percent += Input.GetAxis("Mouse X") * Time.deltaTime * speed;
            percent = Mathf.Clamp(percent, -1f, 1f);
    
            transform.position = Vector3.Lerp(transform.position, new Vector3(maxLeft * percent, transform.position.y, transform.position.z), Time.deltaTime);
        
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.eulerAngles.x, maxAngle * percent, transform.eulerAngles.z), Time.deltaTime);
        }
    }
}
