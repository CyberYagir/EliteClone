using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Core.ActivatorDemo
{
    public class ActivatorDemo : MonoBehaviour
    {
        [SerializeField] private Transform shied;
        [SerializeField] private ChainIKConstraint ik;
        public void Animate()
        {
            StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            while (ik.weight < 1)
            {
                ik.weight += Time.deltaTime;
                yield return null;
            }

            shied.DOScale(Vector3.one * 25, 10);
            
            while (ik.weight > 0)
            {
                ik.weight -= Time.deltaTime * 5;
                yield return null;
            }
        }
    }
}
