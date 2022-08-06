using System;
using System.Collections;
using System.Collections.Generic;
using Core.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<MonoUI> uiObjects;


    private void Update()
    {
        for (int i = 0; i < uiObjects.Count; i++)
        {
            if (uiObjects[i].gameObject.active && uiObjects[i].enabled)
            {
                uiObjects[i].OnUpdate();
            }
        }
    }
}
