using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class ShooterBotLookBack : MonoBehaviour
    {
        [SerializeField] private float reactionTime;
        private float time;
        private bool triggered;
        public Event InBack, OutBack;
        private void Update()
        {
            if (triggered)
            {
                time += Time.deltaTime;
                if (time > reactionTime)
                {
                    transform.DOKill();
                    transform.DOLookAt(ShooterPlayer.Instance.transform.position, 0.5f);
                    time = 0;
                }

                var dir = Vector3.Dot((transform.position - ShooterPlayer.Instance.transform.position).normalized, transform.forward);

                if (dir > 0.5f)
                {
                    InBack.Run();
                }
                else
                {
                    OutBack.Run();
                }
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ShooterPlayer>())
            {
                time = 0;
                triggered = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ShooterPlayer>())
            {
                triggered = false;
                OutBack.Run();
            }
        }
    }
}
