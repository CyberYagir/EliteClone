using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase;
using Pathfinding;
using UnityEngine;

namespace Core.TDS
{
    public class InteractPoint : MonoBehaviour
    {
        [SerializeField] protected List<TDSPointsWaker> persons = new List<TDSPointsWaker>();
        [SerializeField] protected List<TDSPointsWaker> moving = new List<TDSPointsWaker>();
        [SerializeField] private int count;
        public virtual bool IsEmpty()
        {
            return (moving.Count + persons.Count < count);
        }

        public virtual void Calculate()
        {
            for (int i = 0; i < moving.Count; i++)
            {
                if (moving[i].ArriveCheck())
                {
                    OnArrive(moving[i]);
                }
            }
        }

        public virtual bool AddToMove(TDSPointsWaker move)
        {
            if (IsEmpty())
            {
                moving.Add(move);
                return true;
            }

            return false;
        }
        
        public virtual void OnArrive(TDSPointsWaker move)
        {
            moving.Remove(move);
            persons.Add(move);
        }

        public virtual void RemovePerson(TDSPointsWaker move)
        {
            persons.Remove(move);
            move.RemoveAnim();
        }

        public Transform GetLastPerson()
        {
            if (persons.Count == 0) return null;
            return persons[0].transform;
        }
    }
}
