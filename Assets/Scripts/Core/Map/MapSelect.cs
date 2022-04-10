using System;
using System.Collections;
using System.Collections.Generic;
using Core.Map;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Core.Map
{
    public class MapSelect : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject point;
        public RectTransform rect;

        void Update()
        {
            point.transform.position = CalcPos(MapGenerator.selected.transform.position);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider != null)
                        MapGenerator.Instance.ChangeSelected(hit.transform.gameObject);
                }
            }
        }

        public Vector3 CalcPos(Vector3 worldPos)
        {
            var pointPos = camera.WorldToScreenPoint(worldPos, Camera.MonoOrStereoscopicEye.Mono);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pointPos, camera, out Vector2 pos);
            return rect.TransformPoint(pos);
        }


    }
}
