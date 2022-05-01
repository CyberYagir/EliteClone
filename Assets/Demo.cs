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
    private void Awake()
    {
        StartCoroutine(DemoWaiter());
    }

    IEnumerator DemoWaiter()
    {
        yield return new WaitForSecondsRealtime(9f);
        textField.DOAnchorPos(new Vector2(544, 186), 0.2f).SetUpdate(UpdateType.Late);

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

    public void OnSecond()
    {
        firstModel.SetActive(false);
        secondModel.SetActive(true);
    }
    
    IEnumerator StartTime()
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
}
