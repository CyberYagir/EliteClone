using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Garage
{
    public class CarController : MonoBehaviour
    {
        [Serializable]
        public class Wheels
        {
            public List<Transform> wheels = new List<Transform>();
        }
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private float speed, rotspeed;
        [SerializeField] private Renderer tracks;
        [SerializeField] private List<Wheels> wheels;
        private void Update()
        {
            rigidbody.centerOfMass = Vector3.down;
            var oldY = rigidbody.velocity.y;
            rigidbody.velocity = (((transform.forward * InputService.GetAxis(KAction.Vertical))) * speed * Time.deltaTime);
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, oldY, rigidbody.velocity.z);
            rigidbody.MoveRotation(transform.rotation * Quaternion.Euler(0, rotspeed * Time.deltaTime * InputService.GetAxis(KAction.Horizontal), 0));

            var mats = tracks.materials;
            var sp = InputService.GetAxisRaw(KAction.Vertical) * Time.deltaTime * speed;
            var sidesp = InputService.GetAxisRaw(KAction.Horizontal) * Time.deltaTime * speed;

            AddScroll(mats, 0, sp + sidesp);
            AddScroll(mats, 2, sp - sidesp);

            RotateWheels(0, sp + sidesp);
            RotateWheels(1, -sp + sidesp);
        
            tracks.materials = mats;

        }

        public void AddScroll(Material[] mats, int id, float add)
        {
        
            mats[id].SetTextureOffset("_BaseColorMap", new Vector2(0, mats[id].GetTextureOffset("_BaseColorMap").y - add));
        }

        public void RotateWheels(int id, float add)
        {
            for (int i = 0; i < wheels[id].wheels.Count; i++)
            {
                wheels[id].wheels[i].Rotate(Vector3.right, add, Space.Self);
            }
        }
    
    
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(rigidbody.worldCenterOfMass, 1);
        }
    }
}
