using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.TDS
{
    public class ShooterPlayer : Shooter
    {
        public static ShooterPlayer Instance;
        public Camera followCamera;

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }
    }
}
