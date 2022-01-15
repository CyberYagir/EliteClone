using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private float eventTime;
    [SerializeField] private Event OnStart = new Event();
    [SerializeField] private Event OnStarted = new Event();
    private void Awake()
    {
        OnStart.Invoke();
        StartCoroutine(WaitForEvent());
    }

    IEnumerator WaitForEvent()
    {
        yield return new WaitForSeconds(eventTime);
        OnStarted.Invoke();
    }
    
    public void SetColor(float alpha)
    {
        image.color = new Color(0,0,0,alpha);
    }
    public void Fade(float toValue = 1)
    {
        image.DOFade(toValue, 0.5f);
    }
}
