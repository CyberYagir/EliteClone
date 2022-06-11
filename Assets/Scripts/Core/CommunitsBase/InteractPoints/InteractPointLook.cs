using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using DG.Tweening;
using UnityEngine;

namespace Core.CommunistsBase.Intacts
{
    public class InteractPointLook : InteractPoint
    {
        [SerializeField] private float YRot;

        [SerializeField] private string animation;

        private TDSPointsWaker lastWalker;
        
        [SerializeField] private Event<TDSPointsWaker> OnArriveEvent = new Event<TDSPointsWaker>();
        
        public override void OnArrive(TDSPointsWaker move)
        {
            base.OnArrive(move);
            int hash = Animator.StringToHash(animation);
            move.SetAnim(hash);
            move.transform.DORotate(new Vector3(0, YRot, 0), 1f);
            lastWalker = move;
            move.GetInteract().ActionEvent.AddListener(Punch);
        }

        public void Punch()
        {
            OnArriveEvent.Run(lastWalker);
        }
    }
}
