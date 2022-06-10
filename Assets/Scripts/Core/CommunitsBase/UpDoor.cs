using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using DG.Tweening;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class UpDoor : MonoBehaviour
    {
        private bool triggered;
        [SerializeField] private Transform mesh;
        [SerializeField] private float up;
        [SerializeField] private List<AlphaController> controllers;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<ShooterPlayer>())
            {
                StopAllCoroutines();
                mesh.DOKill();
                mesh.DOLocalMoveY(up, 2);
                triggered = true;
                for (int i = 0; i < controllers.Count; i++)
                {
                    controllers[i].overrideActive = true;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<ShooterPlayer>())
            {
                mesh.DOKill();
                mesh.DOLocalMoveY(0, 2);
                mesh.gameObject.layer = LayerMask.NameToLayer("Main");
                triggered = false;
                
                for (int i = 0; i < controllers.Count; i++)
                {
                    controllers[i].overrideActive = false;
                }
            }
        }
    }
}
