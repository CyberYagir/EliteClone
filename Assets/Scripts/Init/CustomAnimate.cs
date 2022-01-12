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

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    private void Update()
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, !reverse ? endPos : startPos, speed * Time.deltaTime);
    }
}
