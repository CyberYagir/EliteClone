using DG.Tweening;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class ShieldActivator : MonoBehaviour
    {
        [SerializeField] private Transform shield;

        private bool active;

        public bool isActive
        {
            set
            {
                active = value;
                shield.DOScale(active ? Vector3.one : Vector3.zero, 0.5f);
            }
            get => active;
        }
    }
}
