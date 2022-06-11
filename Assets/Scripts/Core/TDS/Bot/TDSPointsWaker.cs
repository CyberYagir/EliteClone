using System;
using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase;
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
        private void Start()
        {
            pointManager = GetComponentInParent<InteractPointManager>();
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
                }
            }
        }

        public bool ArriveCheck()
        {
            if (!isArrived)
            {
                var botPos = new Vector2(transform.position.x, transform.position.z);
                var targPos = new Vector2(setter.target.position.x, setter.target.position.z);
                if (Vector2.Distance(botPos, targPos) < 0.1f)
                {
                    isArrived = true;
                    return true;
                }
            }

            return false;
        }

        public void SetAnim(int animHash)
        {
            animator.Play(animHash, 1);
            StartCoroutine(ChangeLayer(1, 2, 0.5f, true));

        }

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
