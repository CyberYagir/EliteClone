using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Core.CommunistsBase.OutDemo
{
    [ExecuteInEditMode]
    public class CameraLook : MonoBehaviour
    {
        [SerializeField] private Transform point;

        private void Update()
        {
            if (Application.isPlaying)
            {
                transform.DOLookAt(point.position, 0.2f);
            }
            else
            {
                transform.LookAt(point.position);
            }
        }
    }
}
