using Core.TDS;
using DG.Tweening;
using UnityEngine;

namespace Core.Demo
{
    public class DemoDoor : MonoBehaviour
    {
        public bool enable;
        public Event OnOpen = new Event();
        private void OnTriggerStay(Collider other)
        {
            if (enable && other.GetComponent<ShooterPlayer>() && !other.isTrigger)
            {
                enable = false;
                transform.DOLocalRotate(new Vector3(0, 0, 90), 5);
                OnOpen.Run();
            }
        }
    }
}
