using System.Collections;
using UnityEngine;

namespace Core.Core.Demo
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
            while (Time.timeScale > 0)
            {
                if (Time.timeScale - Time.unscaledDeltaTime * speed < 0)
                {
                    Time.timeScale = 0;
                    yield break;
                }
                Time.timeScale -= Time.unscaledDeltaTime * speed;
                yield return null;
            }
        }
    }
}
