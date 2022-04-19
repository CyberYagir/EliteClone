using System.Collections.Generic;
using Core.Game;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class ShipMeshManager : MonoBehaviour
    {
        public List<ShipMeshSlot> slots;
        private ItemShip currentShip;
        public Event OnInit = new Event();
        public void InitSlots(ItemShip itemShip)
        {
            for (int i = 0; i < itemShip.slots.Count; i++)
            {
                var findWithID = slots.Find(x => x.slotID == itemShip.slots[i].uid);
                if (findWithID != null)
                {
                    findWithID.SetMesh(itemShip.slots[i]);
                }
            }
            
            OnInit.Run();
        }

        public void SetCurrentShip(ItemShip itemShip)
        {
            currentShip = itemShip;
        }
    
        public void SetInitSlotsWithoutArgs()
        {
            InitSlots(currentShip);
        }
    }
}
