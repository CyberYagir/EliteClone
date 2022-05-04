using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class AutoRepath : MonoBehaviour
    {
        [SerializeField] private float repathRate;
        private void Start()
        {
            StartCoroutine(Repath());
        }

        IEnumerator Repath()
        {
            while (true)
            {
                yield return new WaitForSeconds(repathRate);
                AstarPath.active.Scan();
            }
        }
    }
}
