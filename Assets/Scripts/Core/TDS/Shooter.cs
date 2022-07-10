using UnityEngine;

namespace Core.TDS
{
    public class Shooter : MonoBehaviour
    {
        public ShooterPlayerActions attack { get; private set; }
        public ShooterAnimator animator { get; private set; }
        public ShooterData data { get; private set; }
        public RagdollActivator ragdoll { get; private set; }
        
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
            ragdoll = GetComponent<RagdollActivator>();
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
            ragdoll.ActivateRagdoll(animator.Get(), force);
            OnDeath.Run();  
        }

    }
}