using System;
using System.Collections;
using System.Collections.Generic;
using Core.UI;
using UnityEngine;

namespace Core.TDS.UI
{
    public class UIWeaponDisplay : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform item;

        private List<UIWeaponDisplayItem> displayItems = new List<UIWeaponDisplayItem>(5);
        private int lastWeapon;
        private void Awake()
        {
            GetComponentInParent<ShooterWeaponSelect>().OnChangeCurrentWeapon.AddListener(OnChangeWeapon);
        }

        public void Redraw()
        {
            displayItems.Clear();
            item.gameObject.SetActive(false);
            UITweaks.ClearHolder(holder);
            int id = 0;
            foreach (var it in ShooterPlayer.Instance.inventory.items)
            {
                var obj = Instantiate(item, holder).GetComponent<UIWeaponDisplayItem>();
                obj.Init(it, id+1);
                displayItems.Add(obj);
                obj.gameObject.SetActive(true);
                id++;
            }

            OnChangeWeapon(lastWeapon);
        }

        public void OnChangeWeapon(int val)
        {
            lastWeapon = val;
            for (int i = 0; i < displayItems.Count; i++)
            {
                displayItems[i].Active(val == i);
            }
        }
    }
}
