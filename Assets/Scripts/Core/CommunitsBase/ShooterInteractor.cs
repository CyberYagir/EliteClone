using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Core.CommunistsBase.Intacts
{
    public class ShooterInteractor : MonoBehaviour
    {
        private TDSPointsWaker walker;
        private bool triggered;
        private Transform target;

        [SerializeField] private bool lookAt = true;
        
        [SerializeField] private Event Action;
        [SerializeField] private Event Disable;


        private Quaternion startRotation;

        private void Start()
        {
            startRotation = transform.rotation;
        }

        private void FixedUpdate()
        {
            if (triggered && lookAt)
            {
                var pos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                var targetRotation = Quaternion.LookRotation(pos - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
            }
        }


        public void ReturnRotation()
        {
            transform.DORotateQuaternion(startRotation, 1f);
        }

        public void TriggerAction()
        {
            Action.Invoke();
        }

        public void UnTrigger()
        {
            Disable.Run();
        }
        private void Awake()
        {
            walker = GetComponent<TDSPointsWaker>();
        }

        private void OnDisable()
        {
            triggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggered && this.enabled)
            {
                var inters = other.GetComponentInParent<ShooterPlayerInteractor>();
                if (inters)
                {
                    if (walker != null)
                    {
                        if (ShooterPlayer.Instance.inventory.items.Count == 0)
                        {
                            inters.AddInteractor(this);
                            StartCoroutine(ShooterAnimator.ChangeLayer(walker.GetAnimator(), 2, 2.5f, 0, true));
                            triggered = true;
                            target = inters.transform;
                        }
                    }
                    else
                    {
                        inters.AddInteractor(this);
                        triggered = true;
                        target = inters.transform;
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggered && this.enabled && !ShooterPlayerInteractor.interacted)
            {
                var inters = other.GetComponentInParent<ShooterPlayerInteractor>();
                if (inters)
                {
                    inters.DestroyInteractor(this);
                    if (walker != null)
                    {
                        StartCoroutine(ShooterAnimator.ChangeLayer(walker.GetAnimator(), 2, 2.5f, 0, false));
                    }

                    triggered = false;
                }
            }
        }
    }
}
