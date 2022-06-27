using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase.Intacts
{
    public class ShooterInteractor : MonoBehaviour
    {
        private TDSPointsWaker walker;
        private bool triggered;
        private Transform target;

        private void FixedUpdate()
        {
            if (triggered)
            {
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            }
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
                    StartCoroutine(walker.ChangeLayer(2, 2.5f, 0, true));
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
                    StartCoroutine(walker.ChangeLayer(2, 2.5f, 0, false));
                    triggered = false;
                }
            }
        }
    }
}
