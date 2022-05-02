using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Demo : MonoBehaviour
{
    [SerializeField] private RectTransform textField;
    [SerializeField] private GameObject firstModel, secondModel;
    [SerializeField] private Animator secondAnimator;
    [SerializeField] private GameObject cube, mars;
    [SerializeField] private ParticleSystem shootParticles;
    [SerializeField] private Camera camera;

    private float textShowTimer;
    private bool timerShowed;
    
    private float time;
    private void Awake()
    {
        StartCoroutine(MarsWaiter());
    }

    public void ShowText()
    {
        textField.DOAnchorPos(new Vector2(544, 186), 0.2f).SetUpdate(UpdateType.Late);
        StartCoroutine(DemoWaiter());
    }
    

    IEnumerator DemoWaiter()
    {
        
        while (!Input.GetKeyDown(KeyCode.Return) || textField.GetComponent<TMP_InputField>().text == "")
        {
            yield return null;
        }

        textField.DOAnchorPos(new Vector2(544, -1000), 0.2f).SetUpdate(UpdateType.Late);
        StartCoroutine(StartTime());

        yield return new WaitForSecondsRealtime(2f);

        secondAnimator.Play("Death", 2);
        float weight = 0;
        while (secondAnimator.GetLayerWeight(2) < 1)
        {
            weight += Time.unscaledDeltaTime * 2;
            secondAnimator.SetLayerWeight(2, weight);
            yield return null;
        }
    }

    IEnumerator MarsWaiter()
    {
        yield return new WaitForSeconds(9.3f);
        yield return StartCoroutine(StopTimer());
        camera.GetComponent<Animator>().enabled = true;
        
    }

    public void ShowMars()
    {
        mars.SetActive(true);
        cube.SetActive(false);
        shootParticles.gameObject.SetActive(false);
        mars.SetActive(true);
        cube.SetActive(false);
    }
    
    public void OnSecond()
    {
        firstModel.SetActive(false);
        secondModel.SetActive(true);
    }
    
    public IEnumerator StartTime()
    {
        while (Time.timeScale < 1)
        {
            if (Time.timeScale + Time.unscaledDeltaTime * 1 > 1)
            {
                Time.timeScale = 1;
                yield break;
            }
            Time.timeScale += Time.unscaledDeltaTime * 1;
            yield return null;
        }
    }
    
    IEnumerator StopTimer()
    {
        while (Time.timeScale > 0)
        {
            if (Time.timeScale - Time.unscaledDeltaTime * 2 < 0)
            {
                Time.timeScale = 0;
                yield break;
            }
            Time.timeScale -= Time.unscaledDeltaTime * 2;
            yield return null;
        }
    }
}
