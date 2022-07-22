using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.CommunistsBase.OutDemo
{
    [ExecuteInEditMode]
    public class PointPos : MonoBehaviour
    {
        [SerializeField] private Transform comparePoint;

        private void Update()
        {
            comparePoint.transform.position = Vector3.Lerp(comparePoint.transform.position, transform.position, 5 * Time.deltaTime);
        }
    }
}
