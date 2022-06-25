using System;
using UnityEngine;

namespace Core.Demo
{
    public class DemoPlayerCamera : MonoBehaviour
    {
        [SerializeField] protected Vector3 offcet;
        [SerializeField] protected Transform player;
        

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
        }


        public void UpdateCamera(float delta)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offcet, 10 * delta);
        }
    }
}
