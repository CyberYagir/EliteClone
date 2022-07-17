using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Core.TDS
{
    public class RagdollActivator : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> bones;
        [SerializeField] private List<Collider> colliders;
        public bool isActived {get; set;}
        public void AutoRagdoll()
        {
            ActivateRagdoll(GetComponentInChildren<Animator>());
        }

        public void ActivateRagdoll(Animator animator, Vector3 pos = default)
        {
            isActived = true;
            foreach (var rb in bones)
            {
                rb.isKinematic = false;
                if (rb.GetComponent<BoxCollider>())
                {
                    rb.AddExplosionForce(1000, pos, 5);
                }
            }

            var playerCollider = ShooterPlayer.Instance.GetComponent<CapsuleCollider>();
            var layer = LayerMask.NameToLayer("Default");
            foreach (var col in colliders)
            {
                col.enabled = true;
                col.gameObject.layer = layer;
                Physics.IgnoreCollision(playerCollider, col);
            }

            foreach (var mono in GetComponents<MonoBehaviour>())
            {
                mono.enabled = false;
            }

            gameObject.layer = LayerMask.NameToLayer("Main");
            animator.enabled = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<CapsuleCollider>().enabled = false;
            var nav = GetComponent<NavMeshAgent>();
            if (nav != null) nav.enabled = false;
        }

        public void GetAll()
        {
            bones = GetComponentsInChildren<Rigidbody>().ToList();
            colliders = GetComponentsInChildren<Collider>().ToList();
            bones.Remove(GetComponent<Rigidbody>());
            colliders.Remove(GetComponent<CapsuleCollider>());
            colliders.Remove(GetComponent<SphereCollider>());
        }
    }

}