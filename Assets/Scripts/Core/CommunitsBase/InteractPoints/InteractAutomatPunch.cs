using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.CommunistsBase.Intacts.Other
{
    public class InteractAutomatPunch : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void Punch()
        {
            animator.Play("AutomatPunch");
        }
    }
}
