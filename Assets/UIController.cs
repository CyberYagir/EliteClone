using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<MonoUI> uiObjects;

    private void Start()
    {
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].OnStoreData(PlayerDataManager.Instance);
        }
        
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].Init();
        }
    }


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
