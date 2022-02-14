using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class DrawInventory : MonoBehaviour
{
    [SerializeField] private RectTransform holder, item;
    [SerializeField] private UpDownUI upDownUI;
    [SerializeField] private StatsDisplayCanvasRow tonsRow;
    private Cargo cargo;
    private float height;
    private void Awake()
    {
        cargo = Player.inst.cargo;
        cargo.OnChangeInventory += OnChangeItems;
        height = item.sizeDelta.y;
    }

    private void Update()
    {
        holder.anchoredPosition = Vector2.Lerp(holder.anchoredPosition, new Vector2(0, height * upDownUI.selectedIndex), 10 * Time.deltaTime);
    }

    private void OnChangeItems()
    {
        UITweaks.ClearHolder(holder);
 
        for (int i = 0; i < cargo.items.Count; i++)
        {
            var newItem = Instantiate(item, holder);
            var q = newItem.GetComponent<ItemUI>();
            q.Init(cargo.items[i]);
            newItem.gameObject.SetActive(true);
        }
        upDownUI.itemsCount = cargo.items.Count;
        tonsRow.SetValue(Player.inst.cargo.tons, Player.inst.Ship().data.maxCargoWeight, "_");
    }
}
