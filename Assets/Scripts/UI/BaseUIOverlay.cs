using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIOverlay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private UpDownUI behaviour;
    private Color color;

    private void Start()
    {
        color = image.color;
    }

    private void Update()
    {
        image.color = Color.Lerp(image.color, behaviour.enabled ? new Color(0, 0, 0, 0): color, 10 * Time.deltaTime);
    }
}
