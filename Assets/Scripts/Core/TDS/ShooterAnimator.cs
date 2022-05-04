using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Core.TDS
{
    public class ShooterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float maxVelocity;
        [SerializeField] private Rigidbody rigidbody;
        
        
        [SerializeField] private AIPath aiPath;
        
        private static readonly int Horizontal = Animator.StringToHash("horizontal");
        private static readonly int Vertical = Animator.StringToHash("vertical");
        private static readonly int Moving = Animator.StringToHash("moving");

        private void Update()
        {
            Vector3 dir;
            bool canMove;
            if (aiPath == null)
            {
                canMove = rigidbody.velocity.magnitude > 0.01f;
                dir = transform.InverseTransformDirection(rigidbody.velocity);
            }else{
                dir = transform.InverseTransformDirection(aiPath.velocity);
                canMove = aiPath.velocity.magnitude > 0.01f;
            }

            if (canMove)
            {
                animator.SetBool(Moving, true);
                var horizontal = dir.x / maxVelocity;
                if (horizontal < -1) horizontal = -1;
                if (horizontal > 1) horizontal = 1;
                animator.SetFloat(Horizontal, horizontal);

                var vertical = dir.z / maxVelocity;
                if (vertical < -1) vertical = -1;
                if (vertical > 1) vertical = 1;
                animator.SetFloat(Vertical, vertical);
            }
            else
            {
                animator.SetFloat(Vertical, 0);
                animator.SetFloat(Horizontal, 0);
                animator.SetBool(Moving, false);
            }
        }

        public void SetLayerValue(int id, float value)
        {
            animator.SetLayerWeight(id, value);
        }

        public void Disable()
        {
            animator.enabled = false;
        }

        public ref Animator Get()
        {
            return ref animator;
        }
    }

}
