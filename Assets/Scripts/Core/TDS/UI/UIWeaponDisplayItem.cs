using System.Collections;
using System.Collections.Generic;
using Core.Game;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.TDS.UI
{
    public class UIWeaponDisplayItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text key;
        [SerializeField] private Image icon;
        [SerializeField] private RectTransform rect;
        private Item item;


        public void Init(Item it, int id)
        {
            item = it;

            key.text = id.ToString();
            icon.sprite = item.icon;
        }

        public void Active(bool state)
        {
            rect.DOSizeDelta(new Vector2(rect.sizeDelta.x, state ? 108 : 100), 0.3f);
        }
    }
}
