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
    [SerializeField] Vector2 sizeOpen, sizeClose;
    [SerializeField] Vector3 openY, closeY;
    [SerializeField] Tab[] tabs;
    [SerializeField] int tabIndex;
    [SerializeField] bool active;

    public bool Active => active;
    
    private float time = 60;
    private RectTransform rectTransform;
    
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        active = time < 25;
        
        Scroll();
        if (tabs.Length > 0)
        {
            ChangeSelect();
            if (SelectLoop())
            {
                for (int i = 0; i < tabs.Length; i++)
                {
                    if (i == tabIndex)
                    {
                        tabs[i].buttonEffect.over = ButtonEffect.ActionType.Over;
                    }
                    else
                    {
                        tabs[i].buttonEffect.over = ButtonEffect.ActionType.None;
                    }

                    tabs[i].content.SetActive(i == tabIndex);
                }
            }
        }
    }

    public void Scroll()
    {
        rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, !active ? sizeClose : sizeOpen, 10 * Time.deltaTime);
        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, !active ? closeY : openY, 10 * Time.deltaTime);
    }
    public void ChangeSelect()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            time = 0;
            if (active)
            {
                tabIndex--;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            time = 0;
            if (active)
            {
                tabIndex++;
            }
        }
    }
    bool SelectLoop()
    {
        if (tabIndex >= tabs.Length)
        {
            tabIndex = 0;
        }
        if (tabIndex < 0)
        {
            tabIndex = tabs.Length-1;
        }
        if (tabIndex == -1) return false;

        return true;
    }
}
