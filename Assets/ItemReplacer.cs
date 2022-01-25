using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ItemReplacer : MonoBehaviour
{
    [SerializeField] private GameObject icon, replacer;

    public static Slot selectedSlot = null;

    private void Start()
    {
        UpdateVisible();
    }

    private void Update()
    {
        UpdateVisible();
    }

    public void UpdateVisible()
    {
        icon.SetActive(!DragManager.Instance.dragObject);
        replacer.SetActive(DragManager.Instance.dragObject);
    }

    public void Disable()
    {
        icon.SetActive(true);
        replacer.SetActive(false);
    }
    
    public static void SetSelected(Slot sel)
    {
        selectedSlot = sel;
    }
}
