using UnityEngine;

namespace Core.TDS
{
    public class Shooter : MonoBehaviour
    {
        public ShooterPlayerActions attack { get; private set; }
        public ShooterAnimator animator { get; private set; }
        public ShooterData data { get; private set; }

        public bool isDead { get; private set; }

        public Event OnDeath = new Event();

        protected virtual void Awake()
        {
            Init();
        }

        public void Init()
        {
            attack = GetComponent<ShooterPlayerActions>();
            animator = GetComponent<ShooterAnimator>();
            data = GetComponent<ShooterData>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        public void Death(Vector3 force = default)
        {
            isDead = true;
            ActiveRagdoll(force);
            OnDeath.Run();  
        }
        
        public void ActiveRagdoll(Vector3 pos = default)
        {
            foreach (var rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
                if (rb.GetComponent<BoxCollider>())
                {
                    rb.AddExplosionForce(1000, pos, 5);
                }

                rb.gameObject.layer = LayerMask.NameToLayer("Map");
            }
            foreach (var col in GetComponentsInChildren<Collider>())
            {
                col.enabled = true;
            }

            foreach (var mono in GetComponents<MonoBehaviour>())
            {
                mono.enabled = false;
            }
            gameObject.layer = LayerMask.NameToLayer("Main");
            animator.Disable();
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<CapsuleCollider>().enabled = false;
        }
        
    }
}