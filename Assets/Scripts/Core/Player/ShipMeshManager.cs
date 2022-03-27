using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ShipMeshManager : MonoBehaviour
{
    public List<ShipMeshSlot> slots;
    private ItemShip currentShip;
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
