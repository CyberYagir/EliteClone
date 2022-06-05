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
        [SerializeField] private List<DoorExit> exits;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<ShooterPlayer>())
            {
                StopAllCoroutines();
                mesh.DOKill();
                mesh.DOLocalMoveY(up, 2);
                for (int i = 0; i < exits.Count; i++)
                {
                    exits[i]?.gameObject.SetActive(true);
                }
                triggered = true;
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
            }
        }
    }
}
