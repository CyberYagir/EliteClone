using System;
using Core.TDS;
using UnityEngine;

namespace Core.Demo
{
    public class DemoShipYardEnter : MonoBehaviour
    {
        public Event OnEnter = new Event();

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ShooterPlayer>())
            {
                OnEnter.Run();
                Destroy(this);
            }
        }
    }
}
