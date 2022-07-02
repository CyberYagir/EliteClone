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
            startRotation = startRotation;
        }

        private void FixedUpdate()
        {
            if (triggered)
            {
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
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

        private void OnTriggerEnter(Collider other)
        {
            if (!triggered)
            {
                var inters = other.GetComponentInParent<ShooterPlayerInteractor>();
                if (inters)
                {
                    inters.AddInteractor(this);
                    StartCoroutine(ShooterAnimator.ChangeLayer(walker.GetAnimator(), 2, 2.5f, 0, true));
                    triggered = true;
                    target = inters.transform;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggered)
            {
                var inters = other.GetComponentInParent<ShooterPlayerInteractor>();
                if (inters)
                {
                    inters.DestroyInteractor(this);
                    StartCoroutine(ShooterAnimator.ChangeLayer(walker.GetAnimator(), 2, 2.5f, 0, false));
                    triggered = false;
                }
            }
        }
    }
}
