using System;
using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase;
using DG.DemiLib;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Core.TDS
{
    public class TDSPointsWaker : MonoBehaviour
    {
        private InteractPointManager pointManager;
        [SerializeField] private bool isWakingToPoint, isArrived;
        [SerializeField] private Animator animator;
        [SerializeField] private NPCInteract interactor;
        
        [SerializeField] private Range range;
        private NavMeshAgent agent;
        private float time;
        private float cooldown;


        private Transform currentTarget;
        private void Awake()
        {
            pointManager = GetComponentInParent<InteractPointManager>();
            agent = GetComponent<NavMeshAgent>();
            animator.SetBool("Drunk", Random.Range(0, 1f) > 0.7f);
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
            StartCoroutine(ChangeLayer(1, 2, 0.5f, true));
        }

        public void RemoveAnim()
        {           
            StartCoroutine(ChangeLayer(1, 2, 0, false));
        }

        public void SetAnimFloat(string nm, float val) => animator.SetFloat(nm, val);

        public IEnumerator ChangeLayer(int layer, float speed, float delay, bool to = true)
        {
            var weight = animator.GetLayerWeight(layer);
            yield return new WaitForSeconds(delay);
            while (to ? weight < 1 : weight > 0)
            {
                yield return null;
                weight += Time.deltaTime * speed * (to ? 1 : -1);
                animator.SetLayerWeight(layer, weight);
            }
        }

        public NPCInteract GetInteract()
        {
            return interactor;
        }
    }
}
