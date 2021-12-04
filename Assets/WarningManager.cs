using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum WarningTypes
{
    Damage, Heat, Fuel, GoLocation
}

public class WarningManager : MonoBehaviour
{
    public  static  WarningManager instance;
    [SerializeField] private GameObject item;
    [SerializeField] private Transform holder;
    List<WarningTypes> warnings = new List<WarningTypes>();
    
    
    private void Awake()
    {
        instance = this;
    }


    public static void AddWarning(string text, WarningTypes tag)
    {
        if (!instance.warnings.Contains(tag))
        {
            var it = Instantiate(instance.item.gameObject, instance.holder);
            it.GetComponentInChildren<TMP_Text>().text = text;
            it.gameObject.SetActive(true);
            instance.warnings.Add(tag);
            instance.StartCoroutine(instance.WaitForDestroy(tag, it.gameObject));
        }
    }

    IEnumerator WaitForDestroy(WarningTypes tag, GameObject warn)
    {
        yield return new WaitForSeconds(2);
        instance.warnings.Remove(tag);
        Destroy(warn);
    }
}
