using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.TDS
{
    public class ShooterPlayerActions : MonoBehaviour
    {
        
        public Event OnShoot = new Event();
        public void Update()
        {
            if (InputM.GetAxisIsActive(KAction.Shoot))
            {
                OnShoot.Run();
            }
        }
    }
}
