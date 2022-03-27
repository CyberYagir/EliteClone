using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIRemover
{
    public class UIRemoveInit : ModInit
    {
        void LateUpdate()
        {
            if (WorldSpaceObjectCanvas.Instance)
            {
                WorldSpaceObjectCanvas.Instance.gameObject.SetActive(false);
            }   
            if (ShipHologram.Instance)
            {
                ShipHologram.Instance.gameObject.SetActive(false);
            }  
        }
    }
}
