using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.TDS
{
    public class ShooterPlayer : MonoBehaviour
    {
        public static ShooterPlayer Instance;
        public ShooterPlayerActions attack { get; private set; }
        public ShooterAnimator animator { get; private set; }

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            Instance = this;
            attack = GetComponent<ShooterPlayerActions>();
            animator = GetComponent<ShooterAnimator>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }
}
