using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
