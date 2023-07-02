using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.UI
{
    public class DrawInventory : MonoBehaviour
    {
        [SerializeField] private RectTransform holder, item;
        [SerializeField] private UpDownUI upDownUI;
        [SerializeField] private StatsDisplayCanvasRow tonsRow;
        private Cargo cargo;
        private float height;
        private void Awake()
        {
            cargo = PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo;
            cargo.OnChangeInventory += OnChangeItems;
            height = item.sizeDelta.y;  
            upDownUI.OnNavigateChange += ChangeButtonColors;
        }

        private void Start()
        {
            tonsRow.SetValue(PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.tons, PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().data.maxCargoWeight);
        }

        private void Update()
        {
            upDownUI.UpdateObject();
            if (upDownUI.selectedIndex != -1 && drops.Count > upDownUI.selectedIndex) //Чтобы не было ошибок upDownUI.selectedIndex 
            {
                holder.anchoredPosition = Vector2.Lerp(holder.anchoredPosition, new Vector2(0, height * upDownUI.selectedIndex), 10 * Time.deltaTime);
                if (buttons.Count != 0)
                {
                    if (InputService.GetAxisDown(KAction.Drop))
                    {
                        if (!drops[upDownUI.selectedIndex].isDrop)
                        {
                            drops[upDownUI.selectedIndex].StartEdit();
                        }
                        else
                        {
                            drops[upDownUI.selectedIndex].DropItem();
                            return;
                        }
                    }

                    if (drops[upDownUI.selectedIndex].isDrop)
                    {
                        drops[upDownUI.selectedIndex].EditVal();
                    }
                }
            }
        }

        public void ChangeButtonColors()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].over = i == upDownUI.selectedIndex ? ButtonEffect.ActionType.Over : ButtonEffect.ActionType.None;
                if (i != upDownUI.selectedIndex)
                    drops[i].ResetText();
            }
        }

        private List<ButtonEffect> buttons = new List<ButtonEffect>();
        private List<ItemUIDrop> drops = new List<ItemUIDrop>();

        private void OnChangeItems()
        {
            UITweaks.ClearHolder(holder);
            buttons.Clear();
            drops.Clear();
        
        
            for (int i = 0; i < cargo.items.Count; i++)
            {
                var newItem = Instantiate(item, holder);
                var q = newItem.GetComponent<ItemUI>();
                q.Init(cargo.items[i]);
                newItem.gameObject.SetActive(true);
                buttons.Add(newItem.GetComponent<ButtonEffect>());
                drops.Add(newItem.GetComponent<ItemUIDrop>());
            }
            upDownUI.itemsCount = cargo.items.Count;
            tonsRow.SetValue(PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.tons, PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().data.maxCargoWeight);
        }
    }
}
