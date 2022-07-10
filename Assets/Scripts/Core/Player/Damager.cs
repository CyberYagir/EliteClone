using UnityEngine;

namespace Core.PlayerScripts
{
    public class Damager : MonoBehaviour
    {
        public Event OnDamaged;
        public Event OnDeath;

        public void TakeDamage(ref float health, float damage)
        {
            health -= damage;
            OnDamaged.Invoke();
            if (health <= 0) OnDeath.Invoke();
        }

    }
}
