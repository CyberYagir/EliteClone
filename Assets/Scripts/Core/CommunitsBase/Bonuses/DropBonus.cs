using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using DG.Tweening;
using UnityEngine;

namespace Core.CommunistsBase.Bonuses
{
    public class DropBonus : MonoBehaviour
    {
        [SerializeField] protected float bonus;

        private void OnTriggerEnter(Collider other)
        {
            var data = other.GetComponentInParent<ShooterData>();
            if (data && data.GetComponent<ShooterPlayer>())
            {
                AddBonus(data);
                transform.DOScale(Vector3.one * 0.01f, 0.5f).onComplete += () => { Destroy(gameObject); };
                transform.DOMove(data.transform.position + Vector3.up, 0.2f);
                Destroy(this);
            }
        }

        public virtual void AddBonus(ShooterData data)
        {
        }
    }
}
