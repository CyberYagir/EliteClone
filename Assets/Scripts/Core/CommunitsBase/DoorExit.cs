using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class DoorExit : MonoBehaviour
    {
        public GameObject enabledRoom;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<ShooterPlayer>())
            {
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<ShooterPlayer>())
            {
            }
        }
    }
}
