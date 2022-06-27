using System;
using System.Collections;
using System.Collections.Generic;
using Core.Demo;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class PlayerTDSCamera : DemoPlayerCamera
    {

        public static PlayerTDSCamera Instance;
        public enum CameraModes
        {
            Control,
            OutsideControl
        }

        public CameraModes mode;
        private Quaternion startRotation;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            startRotation = transform.rotation;
        }

        public void FixedUpdate()
        {
            if (mode == CameraModes.Control)
            {
                transform.rotation = Quaternion.LerpUnclamped(transform.rotation, startRotation, 10 * Time.fixedDeltaTime);
                UpdateCamera(Time.fixedDeltaTime);
            }
        }

        public static void ChangeMode(CameraModes mode)
        {
            if (Instance == null) return;
            Instance.mode = mode;
        }

        public Camera GetCamera()
        {
            return GetComponent<Camera>();
        }
    }
}