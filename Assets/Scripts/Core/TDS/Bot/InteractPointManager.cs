using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Core.TDS
{
    public class InteractPointManager : MonoBehaviour
    {
        [SerializeField] private List<InteractPoint> point;

        private void Start()
        {
            StartCoroutine(PointsTicks());
        }

        public InteractPoint GetEmptyTarget(TDSPointsWaker setter)
        {
            for (int i = 0; i < point.Count; i++)
            {
                if (point[i].IsEmpty())
                {
                    point[i].AddToMove(setter);
                    return point[i];
                }
            }

            return null;
        }

        IEnumerator PointsTicks()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < point.Count; i++)
                {
                    point[i].Calculate();
                }
            }
        }
    }
}
