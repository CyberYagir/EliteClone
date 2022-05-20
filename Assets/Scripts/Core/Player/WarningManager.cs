using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.PlayerScripts
{
    public enum WarningTypes
    {
        Damage, Heat, Fuel, GoLocation
    }

    public class WarningManager : Singleton<WarningManager>
    {
        [SerializeField] private GameObject item;
        [SerializeField] private Transform holder;
        List<WarningTypes> warnings = new List<WarningTypes>();
    
    
        private void Awake()
        {
            Single(this);
        }


        public static void AddWarning(string text, WarningTypes tag)
        {
            if (!Instance.warnings.Contains(tag))
            {
                var it = Instantiate(Instance.item.gameObject, Instance.holder);
                it.GetComponentInChildren<TMP_Text>().text = text;
                it.gameObject.SetActive(true);
                Instance.warnings.Add(tag);
                Instance.StartCoroutine(Instance.WaitForDestroy(tag, it.gameObject));
            }
        }

        IEnumerator WaitForDestroy(WarningTypes tag, GameObject warn)
        {
            yield return new WaitForSeconds(2);
            Instance.warnings.Remove(tag);
            Destroy(warn);
        }
    }
}