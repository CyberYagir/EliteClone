using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class GarageExplorer : CustomAnimate
{
    [SerializeField] private Transform holder, item;
    void Start()
    {
        Init();
        UpdateList();
    }

    public void UpdateList()
    {
        UITweaks.ClearHolder(holder);
        foreach (var it in GarageDataCollect.Instance.playerData.items)
        {
            var spawned = Instantiate(item, holder).GetComponent<GarageDragDropItem>();
            var findedItem = ItemsManager.GetItem(it);
            spawned.Init(findedItem.icon, findedItem);
            spawned.SetSprite(findedItem.icon);
            spawned.gameObject.SetActive(true);
        }
    }
}
