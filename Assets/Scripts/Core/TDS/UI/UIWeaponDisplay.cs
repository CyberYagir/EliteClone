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



        public void Redraw()
        {
            item.gameObject.SetActive(false);
            UITweaks.ClearHolder(holder);
            int id = 0;
            foreach (var it in ShooterPlayer.Instance.inventory.items)
            {
                var obj = Instantiate(item, holder).GetComponent<UIWeaponDisplayItem>();
                obj.Init(it, id+1);
                obj.gameObject.SetActive(true);
                id++;
            }
            
            
        }
    }
}
