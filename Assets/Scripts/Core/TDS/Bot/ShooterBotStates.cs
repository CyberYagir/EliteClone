using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Core.TDS.Bot
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
        [SerializeField] private ShooterBotAttack attackBehaviour;
        
        private void Start()
        {
            ChangeState(States.Normal);
        }

        private void Update()
        {
            
            if (state == States.Attack)
            {
                attackBehaviour.Calculate();
            }
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
                GetComponent<NavMeshAgent>().enabled = true;
            }
        }

        public States GetState()
        {
            return state;
        }
    }
}
