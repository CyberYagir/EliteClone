using System;
using System.Collections;
using System.Collections.Generic;
using Core.Map;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class MapSelect : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject point;
    private RectTransform rect;
    private void Start()
    {
        camera = Camera.main;
        rect = point.GetComponentInParent<RectTransform>();
    }

    void Update()
    {
        var pointPos = camera.WorldToScreenPoint(MapGenerator.selected.transform.position, Camera.MonoOrStereoscopicEye.Mono);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pointPos, camera, out Vector2 pos);
        
        point.transform.position = rect.TransformPoint(pos);

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

}
