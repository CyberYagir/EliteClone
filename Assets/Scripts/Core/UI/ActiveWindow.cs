using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveWindow : MonoBehaviour
{
    [SerializeField] private bool open;
    [SerializeField] private Vector2 show, hide;
    private RectTransform rect;

    public UnityEvent onOpen;
    public UnityEvent onClose;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = open ? show : hide;
    }

    private void Update()
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, open ? show : hide, Time.deltaTime * 5);
    }

    public void SetOpenClose(bool state)
    {
        if (!state && open)
        {
            onClose.Invoke();
        }else
        if (state && !open)
        {
            onOpen.Invoke();
        }
        open = state;
    }
}
