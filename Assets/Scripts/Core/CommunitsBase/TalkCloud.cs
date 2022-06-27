using System;
using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase;
using UnityEngine;
namespace Core.Dialogs
{
    public class TalkCloud : MonoBehaviour
    {
        private Camera camera;
        private void Start()
        {
            camera = PlayerTDSCamera.Instance.GetCamera();
        }

        private void FixedUpdate()
        {
            transform.LookAt(camera.transform);
        }
    }
}
