using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Map
{
    public class MapSelect : MonoBehaviour
    {
        public enum  MapMode
        {
            Frame, Active
        }

        public static MapSelect Instance;
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject point;
        public static GameObject selected;
        public RectTransform rect;

        private void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (selected != null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    UpdatePoint();
                    var ray = camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider != null)
                            ChangeSelected(hit.transform.gameObject);
                    }
                }
            }
        }

        public void UpdatePoint()
        {
            point.transform.position = CalcPos(selected.transform.position);
        }

        public Vector3 CalcPos(Vector3 worldPos)
        {
            var pointPos = camera.WorldToScreenPoint(worldPos, Camera.MonoOrStereoscopicEye.Mono);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pointPos, camera, out Vector2 pos);
            return rect.TransformPoint(pos);
        }

        public void ChangeSelected(GameObject select)
        {
            var targetPos = new Vector3();
            if (selected == null)
            {
                targetPos = (select.transform.position) + new Vector3(0, 5, -5);
                camera.transform.position = targetPos;
                selected = select;
                return;
            }
            else
            {
                targetPos = select.transform.position + (camera.transform.position - selected.transform.position);
            }

            selected = select;
            if (MapGenerator.mode == MapMode.Frame)
            {
                camera.transform.position = targetPos;
            }
            else
            {
                camera.transform.DOMove(targetPos, 0.5f);
                camera.transform.DORotate(camera.transform.eulerAngles, 0.5f);
            }
        }
    }
}
