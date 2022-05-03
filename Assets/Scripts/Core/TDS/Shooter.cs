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

        public void Death()
        {
            isDead = true;
            ActiveRagdoll();
            OnDeath.Run();  
        }
        
        public void ActiveRagdoll()
        {
            foreach (var rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
            }
            foreach (var col in GetComponentsInChildren<Collider>())
            {
                col.enabled = true;
            }

            foreach (var mono in GetComponents<MonoBehaviour>())
            {
                mono.enabled = false;
            }

            animator.Disable();
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<CapsuleCollider>().enabled = false;
        }
        
    }
}