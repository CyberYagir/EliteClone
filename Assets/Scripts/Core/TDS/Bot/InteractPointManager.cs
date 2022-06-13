using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

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
            int count = 1;
            do
            {
                var id = Random.Range(0, point.Count);
                if (point[id].IsEmpty())
                {
                    point[id].AddToMove(setter);
                    return point[id];
                }

                count++;
            } while (count < 5);

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
