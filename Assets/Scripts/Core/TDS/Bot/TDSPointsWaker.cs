using System;
using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase;
using DG.DemiLib;
using Pathfinding;
using UnityEngine;

namespace Core.TDS
{
    public class TDSPointsWaker : MonoBehaviour
    {
        private InteractPointManager pointManager;
        [SerializeField] private bool isWakingToPoint, isArrived;
        [SerializeField] private AIDestinationSetter setter;
        [SerializeField] private Animator animator;
        [SerializeField] private NPCInteract interactor;
        [SerializeField] private Range range;
        private AIPath aiPath;
        private float time;
        private float cooldown;
        private void Start()
        {
            pointManager = GetComponentInParent<InteractPointManager>();
            aiPath = GetComponent<AIPath>();
        }

        private void FixedUpdate()
        {
            if (!isWakingToPoint)
            {
                var target = pointManager.GetEmptyTarget(this);
                if (target != null)
                {
                    interactor.Clear();
                    setter.target = target.transform;
                    isArrived = false;
                    isWakingToPoint = true;
                    aiPath.enableRotation = true;
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
                    if (setter.target != null)
                    {
                        if (setter.target.TryGetComponent(out InteractPoint point))
                        {
                            point.RemovePerson(this);
                        }else if (setter.target.parent.TryGetComponent(out InteractPoint parentPoint))
                        {
                            parentPoint.RemovePerson(this);
                        }
                    }

                    setter.target = null;
                }
            }
        }

        public void SetTarget(Transform point)
        {
            setter.target = point.transform;
        }
        

        public bool ArriveCheck()
        {
            if (!isArrived)
            {
                var botPos = new Vector2(transform.position.x, transform.position.z);
                var targPos = new Vector2(setter.target.position.x, setter.target.position.z);
                if (Vector2.Distance(botPos, targPos) < 0.5f)
                {
                    aiPath.enableRotation = false;
                    isArrived = true;
                    return true;
                }
                else
                {
                    aiPath.enableRotation = true;
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
