using System;
using Core.Game;
using UnityEngine;

namespace Core.Garage
{
    public class ItemReplacer : MonoBehaviour
    {
        [SerializeField] private GameObject icon, replacer;

        public static Slot selectedSlot;
        public Event<Item> OnItemDrops = new Event<Item>();

        private void Start()
        {
            DragManager.Instance.OnDrop += OnDrop;
            UpdateVisible();
        }

        public void OnDrop(DragManager.DragDropData data)
        {
            if (data.hit == gameObject)
            {
                OnItemDrops.Run(data.item);
            }
        }

        private void Update()
        {
            UpdateVisible();
        }

        public bool ReplaceItem(Item drop, Slot currentSlot)
        {
            if ((float)drop.GetKeyPair(KeyPairValue.Level) <= currentSlot.slotLevel)
            {
                //var massWithoutSlot = GarageDataCollect.Instance.ship.CalcMass() - (float)currentSlot.current.GetKeyPair(KeyPairValue.Mass);
                // if (massWithoutSlot + (float) drop.GetKeyPair(KeyPairValue.Mass) <= GarageDataCollect.Instance.ship.data.maxCargoWeight)
                // {
                // }
                
                GarageDataCollect.Instance.ship.ReplaceSlotItem(drop, currentSlot.uid, GarageDataCollect.Instance.cargo);
                return true;
            }

            return false;
        }
    
        public void UpdateVisible()
        {
            icon.SetActive(!DragManager.Instance.dragObject);
            replacer.SetActive(DragManager.Instance.dragObject);
        }

        public void Disable()
        {
            icon.SetActive(true);
            replacer.SetActive(false);
        }
    
        public static void SetSelected(Slot sel)
        {
            selectedSlot = sel;
        }

        public void ResetSelected()
        {
            selectedSlot = null;
        }
    }
}
