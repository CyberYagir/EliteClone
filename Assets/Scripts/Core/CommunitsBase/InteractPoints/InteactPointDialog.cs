using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using DG.Tweening;
using UnityEngine;

namespace Core.CommunistsBase.Intacts
{
    public class InteactPointDialog : InteractPoint
    {
        [System.Serializable]
        public class DialoguePoint
        {
            public Transform point;
            public TDSPointsWaker person;
        }
        [SerializeField] private List<DialoguePoint> points = new List<DialoguePoint>();

        public override void OnArrive(TDSPointsWaker move)
        {
            base.OnArrive(move);

            if (points.Find(x => x.person == move) == null)
            {
                int pointID;
                do
                {
                    pointID = Random.Range(0, points.Count);
                } while (points[pointID].person != null);

                move.SetTarget(points[pointID].point);
                move.transform.DORotateQuaternion(points[pointID].point.transform.rotation, 0.5f);
                var talkersCount = points.FindAll(x => x.person != null).Count;
                if (talkersCount >= 2)
                {
                    TalkAnim(move);
                }else if (talkersCount == 1)
                {
                    TalkAnim(move);
                    for (int i = 0; i < points.Count; i++)
                    {
                        if (points[i].person != null)
                        {
                            TalkAnim(points[i].person);
                        }
                    }
                }
                else
                {
                    move.SetAnim(Animator.StringToHash("Waiting"));
                }

                points[pointID].person = move;
            }
        }

        public void TalkAnim(TDSPointsWaker move)
        {
            move.SetAnim(Animator.StringToHash("Talk" + Random.Range(1, 5)));
            move.SetAnimFloat("Talking Speed", Random.Range(0.9f, 1.4f));
        }
    }
}
