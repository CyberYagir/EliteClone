using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tab
{
    public ButtonEffect buttonEffect;
    public GameObject content;
}

public class UITabControl : MonoBehaviour
{
    public Tab[] tabs;
    public int tabIndex;

    float time = 60;
    RectTransform rectTransform;
    public Vector2 sizeOpen, sizeClose;
    public Vector3 openY, closeY;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, time > 25 ? sizeClose : sizeOpen, 10 * Time.deltaTime);

        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, time > 25 ? closeY : openY, 10 * Time.deltaTime);

        if (tabs.Length == 0) return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            time = 0;
            tabIndex--;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            time = 0;
            tabIndex++;
        }

        if (tabIndex >= tabs.Length)
        {
            tabIndex = 0;
        }
        if (tabIndex < 0)
        {
            tabIndex = tabs.Length-1;
        }

        if (tabIndex == -1) return;

        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].buttonEffect.over = i == tabIndex;
            tabs[i].content.SetActive(i == tabIndex);
        }
    }
}
