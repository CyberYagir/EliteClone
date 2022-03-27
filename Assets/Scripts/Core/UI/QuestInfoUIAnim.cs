using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfoUIAnim : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private UpDownUI updown;
    [SerializeField] private float width, startWidth, transition;
    [SerializeField] private Image background;
    [SerializeField] private Color backActiveColor;
    
    private void Update()
    {
        rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, new Vector2(updown.enabled ? width : startWidth, rect.sizeDelta.y), transition * Time.deltaTime);
        background.color = Color.Lerp(background.color, updown.enabled ? backActiveColor : new Color(), transition * Time.deltaTime);
    }
}
