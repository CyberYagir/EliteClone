using UnityEngine;

namespace Core.Garage
{
    public class GarageShipRotator : MonoBehaviour
    {
        [SerializeField] private float speed = 5, gravity = 1;
        private float velocity;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                velocity = 0;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                velocity -= Input.GetAxis("Mouse X") * speed * Time.deltaTime;
            }
            transform.Rotate(Vector3.up * velocity);
            velocity = Mathf.Lerp(velocity, 0, gravity * Time.deltaTime);
        }
    }
}
