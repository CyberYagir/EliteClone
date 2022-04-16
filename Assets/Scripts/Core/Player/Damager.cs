using UnityEngine;

namespace Core.PlayerScripts
{
    public class Damager : MonoBehaviour
    {
        [HideInInspector]
        public Event OnDamaged;
        [HideInInspector]
        public Event OnDeath;

        public void TakeDamage(ref float health, float damage)
        {
            health -= damage;
            OnDamaged.Invoke();
        }

    }
}
