using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActiveControlUI : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] left, right;
    private bool skipFrame;
    private ButtonEffect effect;

    private void Start()
    {
        effect = GetComponent<ButtonEffect>();
    }

    public void SkipFrame()
    {
        skipFrame = true;
    }
    private void LateUpdate()
    {
        if (skipFrame)
        {
            skipFrame = false;
            return;
        }
        if (InputM.GetAxisDown(KAction.TabsHorizontal))
        {
            enabled = false;
            if (InputM.GetAxisRaw(KAction.TabsHorizontal) < 0)
            {
                ActiveMono(true, left);
                ActiveMono(false, right);
            }
            if (InputM.GetAxisRaw(KAction.TabsHorizontal) > 0)
            {
                ActiveMono(false, left);
                ActiveMono(true, right);
            }
            effect.over = ButtonEffect.ActionType.None;
        }
        else
        {
            GetComponent<ButtonEffect>().over = ButtonEffect.ActionType.Over;

            if (InputM.GetAxisDown(KAction.Select))
            {
                GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    
    
    public void ActiveMono(bool state, MonoBehaviour[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i].enabled = state;
        }
    }
}
