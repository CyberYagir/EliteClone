using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveWindow : MonoBehaviour
{
    [SerializeField] bool open;
    public Vector2 show, hide;
    RectTransform rect;

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
        if (state == false && open == true)
        {
            onClose.Invoke();
        }
        if (state == true && open == false)
        {
            onOpen.Invoke();
        }
        open = state;
    }
}
