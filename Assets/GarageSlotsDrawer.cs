using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class GarageSlotsDrawer : MonoBehaviour
{
    [Serializable]
    public class Icon
    {
        public ItemType type;
        public Sprite icon;
    }
    [SerializeField] private Transform holder, item;
    [SerializeField] private List<Icon> icons = new List<Icon>();
    private void Awake()
    {
        GarageGenerator.OnChangeShip += Draw;
    }
    public void Draw()
    {
        item.gameObject.SetActive(false);
        
        var vl = gameObject.GetComponent<VerticalLayoutGroup>();
        var csf = gameObject.GetComponent<ContentSizeFitter>();
        vl.enabled = true;
        csf.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        
        UITweaks.ClearHolder(holder);
        foreach (var slot in GarageGenerator.Instance.GetShip().slots)
        {
            var button = Instantiate(item.gameObject, holder).GetComponent<GarageSlotItem>();
            button.gameObject.SetActive(true);
            button.SetImage(icons.Find(x=>x.type == slot.slotType).icon);
            button.Init(slot.current, slot);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        
        vl.enabled = false;
        csf.enabled = false;
    }
}
