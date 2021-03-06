using System;
using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase;
using Core.PlayerScripts;
using DG.DemiLib;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Core.TDS
{
    public class TDSPointsWaker : MonoBehaviour, IDamagable
    {
        private InteractPointManager pointManager;
        [SerializeField] private bool isWakingToPoint, isArrived;
        [SerializeField] private Animator animator;
        [SerializeField] private NPCInteract interactor;
        [SerializeField] private Range range;


        private float health = 1;
        private NavMeshAgent agent;
        private float time;
        private float cooldown;

        private Damager damager;


        private Transform currentTarget;
        private void Awake()
        {
            pointManager = GetComponentInParent<InteractPointManager>();
            agent = GetComponent<NavMeshAgent>();
            animator.SetBool("Drunk", Random.Range(0, 1f) > 0.7f);
            damager = GetComponent<Damager>();
        }
        
        private void FixedUpdate()
        {
            if (!isWakingToPoint)
            {
                var target = pointManager.GetEmptyTarget(this);
                if (target != null)
                {
                    agent.enabled = true;
                    currentTarget = target.transform;
                    agent.SetDestination(currentTarget.position);
                    interactor.Clear();
                    currentTarget = target.transform;
                    isArrived = false;
                    isWakingToPoint = true;
                    agent.updateRotation = true;
                    cooldown = range.RandomWithin();
                    time = 0;
                }
            }

            if (isArrived && isWakingToPoint)
            {
                time += Time.deltaTime;
                if (time >= cooldown)
                {
                    isWakingToPoint = false;
                    if (currentTarget != null)
                    {
                        if (currentTarget.TryGetComponent(out InteractPoint point))
                        {
                            point.RemovePerson(this);
                        }else if (currentTarget.transform.parent.TryGetComponent(out InteractPoint parentPoint))
                        {
                            parentPoint.RemovePerson(this);
                        }
                    }

                    currentTarget = null;
                }
            }
        }

        public void SetTarget(Transform point)
        {
            currentTarget = point;
        }
        

        public bool ArriveCheck()
        {
            if (!isArrived)
            {
                var botPos = new Vector2(transform.position.x, transform.position.z);
                var targPos = new Vector2(currentTarget.position.x, currentTarget.position.z);
                if (Vector2.Distance(botPos, targPos) < 0.5f)
                {
                    agent.enabled = false;
                    isArrived = true;
                    return true;
                }
                else
                {
                    agent.enabled = true;
                }
            }

            return false;
        }

        public void SetAnim(int animHash)
        {
            animator.Play(animHash, 1);
            StartCoroutine(ShooterAnimator.ChangeLayer(animator, 1, 2, 0.5f, true));
        }

        public void RemoveAnim()
        {           
            StartCoroutine(ShooterAnimator.ChangeLayer(animator, 1, 2, 0, false));
        }

        public void SetAnimFloat(string nm, float val) => animator.SetFloat(nm, val);



        public NPCInteract GetInteract()
        {
            return interactor;
        }

        public void SetAnimBool(string istalk, bool state)
        {
            animator.SetBool(istalk, state);
        }
        public void SetAnimBool(int istalk, bool state)
        {
            animator.SetBool(istalk, state);
        }


        public Animator GetAnimator()
        {
            return animator;
        }

        public void TakeDamage(float damage)
        {
            damager.TakeDamage(ref health, damage);
        }
    }
}
