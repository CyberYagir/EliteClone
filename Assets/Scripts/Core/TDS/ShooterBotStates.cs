using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.TDS
{
    public class ShooterBotStates : MonoBehaviour
    {
        public enum States
        {
            Normal,
            Attack
        }

        [SerializeField] private States state;
        [SerializeField] private GameObject weapon, weaponSpine;
        [SerializeField] private Animator animator;
        
        private void Start()
        {
            ChangeState(States.Normal);
        }

        public void ChangeState(States newState)
        {
            state = newState;
            
            if (state == States.Normal){
                animator.SetLayerWeight(1,0);
                weapon.SetActive(false);
                weaponSpine.SetActive(true);
            }
            else
            {
                animator.SetLayerWeight(1,1);
                weapon.SetActive(true);
                weaponSpine.SetActive(false);
            }
        }

    }
}
