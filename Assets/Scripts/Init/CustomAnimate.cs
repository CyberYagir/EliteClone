using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimate : MonoBehaviour
{
    [SerializeField] private Vector2 endPos;
    [SerializeField] public bool reverse;
    [SerializeField] private float speed;

    private RectTransform rect;
    private Vector2 startPos;

    private bool inited;
    private void Init()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
        inited = true;
    }

    public void CustomUpdate()
    {
        if (!inited)
        {
            Init();
        }

        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, !reverse ? endPos : startPos, speed * Time.deltaTime);
        if (reverse)
            gameObject.SetActive(Vector2.Distance(rect.anchoredPosition, !reverse ? endPos : startPos) > 5);
        else 
            gameObject.SetActive(true);
    }
}
