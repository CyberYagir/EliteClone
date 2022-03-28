using UnityEngine;

namespace Core.Init
{
    public class InitAnimateEnv : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed;
        void Update()
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);
        }
    }
}
