using System.Collections;
using UnityEngine;

namespace Core.Demo
{
    public class DemoStopTime : MonoBehaviour
    {
        [SerializeField] private float speed;
        public void StopTime()
        {
            StartCoroutine(StopTimer());
        }


        IEnumerator StopTimer()
        {
            yield return StartCoroutine(FindObjectOfType<Demo>().StopTimer());
        }
    }
}
