using System.Collections;
using System.Collections.Generic;
using Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.TDS.UI
{
    public class UIWeaponDisplayItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text key;
        [SerializeField] private Image icon;
        private Item item;


        public void Init(Item it, int id)
        {
            item = it;

            key.text = id.ToString();
            icon.sprite = item.icon;
        }
    }
}
