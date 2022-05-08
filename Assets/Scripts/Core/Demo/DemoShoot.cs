using UnityEngine;

namespace Core.Demo
{
    public class DemoShoot : MonoBehaviour
    {
        [SerializeField] private GameObject particles;


        public void EnableParticles()
        {
            particles.gameObject.SetActive(true);
        }
    }
}
