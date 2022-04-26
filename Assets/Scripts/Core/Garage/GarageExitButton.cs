using System.Collections;
using System.Collections.Generic;
using Core.Garage;
using UnityEngine;

namespace Core.Garage
{
    public class GarageExitButton : MonoBehaviour
    {
        [SerializeField] private List<GarageSlotDataExplorer> explorers;

        public Event OnClick = new Event();
        
        public void Exit()
        {
            for (int i = 0; i < explorers.Count; i++)
            {
                if (explorers[i].IsAllOk() == false)
                {
                    ShipyardError.Instance.ThrowError("Ship data exceeds limits");
                    return;
                }
            }
            
            OnClick.Run();
        }


        public void SaveIfCan()
        {
            for (int i = 0; i < explorers.Count; i++)
            {
                if (explorers[i].IsAllOk() == false)
                {
                    ShipyardError.Instance.ThrowError("Ship data exceeds limits");
                    return;
                }
            }
            
            GarageDataCollect.Instance.Save();
        }
    }
}
