using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    [System.Serializable]
    public class IconType
    {
        public Sprite icon;
        public Fraction fraction;
    }
    private RectTransform rect;
    [SerializeField] private float height = 1400;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Transform charactersListHolder, characterItem;
    [SerializeField] private List<IconType> icons;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0);
    }

    private void Update()
    {
        Animation();
        if (Player.inst.land.isLanded)
        {
            var date = DateTime.Now.Date.AddYears(1025);
            infoText.text = $"Date: {date.ToString("d")}\n" +
                            $"Time: {date.ToString("h:mm:ss")}\n" +
                            $"Credits: Empty";
        }
    }

    public void ClearUI()
    {
        
    }

    public void Animation()
    {
        rect.sizeDelta = Vector2.Lerp(rect.sizeDelta,  new Vector2(rect.sizeDelta.x, Player.inst.land.isLanded ? height : 0), 5 * Time.deltaTime);
    }
}
