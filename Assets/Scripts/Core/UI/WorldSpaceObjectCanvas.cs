using System;
using System.Collections.Generic;
using System.Linq;
using Core.PlayerScripts;
using Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class WorldSpaceObjectCanvas : Singleton<WorldSpaceObjectCanvas>
    {
        [SerializeField] private GameObject pointPrefab, contactPrefab;
        [SerializeField] private TMP_Text contactText;
        private List<DisplaySpaceObject> spaceObjects = new List<DisplaySpaceObject>();
    
    
        private Camera camera;
        private bool skipFrame;
        private bool enablePoints = true;
        [SerializeField] private GraphicRaycaster canvasRaycaster;
        public class DisplaySpaceObject
        {
            public WorldSpaceObject Obj;
            public WorldSpaceCanvasItem CanvasPoint;
            public bool isSystem;
        }


        private void Awake()
        {
            Single(this);
            Player.OnSceneChanged += UpdateList;
        }


        private void Start()
        {
            camera = PlayerDataManager.Instance.WorldHandler.ShipPlayer.GetCamera();
        }

        public bool SetActiveObjects()
        {
            if (enablePoints != !PlayerDataManager.Instance.WorldHandler.ShipPlayer.land.isLanded)
            {
                foreach (var wsp in spaceObjects)
                {
                    wsp.CanvasPoint.gameObject.SetActive(!PlayerDataManager.Instance.WorldHandler.ShipPlayer.land.isLanded);
                }
                enablePoints = !PlayerDataManager.Instance.WorldHandler.ShipPlayer.land.isLanded;
            }

            return enablePoints;
        }
    
        public void SkipFrame()
        {
            skipFrame = true;
        }
    
        public void UpdateList()
        {
            spaceObjects = new List<DisplaySpaceObject>();
            foreach (Transform tr in transform)
            {
                if (tr.gameObject.active)
                {
                    Destroy(tr.gameObject);
                }
            }
            foreach (var wsp in SolarStaticBuilder.Objects)
            {
                SpawnPoint(wsp);
            }

            foreach (var point in FindObjectsOfType<SolarSystemPoint>())
            {
                SpawnPoint(point.GetComponent<WorldSpaceObject>());
            }

            spaceObjects = spaceObjects.OrderBy(x => x.isSystem).ToList();
            UpdatePoints();
        }

        public void SpawnPoint(WorldSpaceObject wsp)
        {
            var obj = Instantiate(pointPrefab, transform).GetComponent<WorldSpaceCanvasItem>();
            spaceObjects.Add(new DisplaySpaceObject {Obj = wsp, CanvasPoint = obj});
            obj.Init(wsp);
        }
        private void LateUpdate()
        {
            if (!skipFrame)
            {
                UpdatePoints();
            }
            else
            {
                skipFrame = false;
            }
        }
    
        public void UpdatePoints()
        {
            var pointer = new PointerEventData(EventSystem.current);
            if (SetActiveObjects())
            {
                var target = PlayerDataManager.Instance.WorldHandler.ShipPlayer.GetTarget();
                if (target != null && target.TryGetComponent(out ContactObject contact))
                {
                    var angle = Vector3.Angle(target.transform.position - camera.transform.position, camera.transform.forward);
                    if (angle < 60)
                    {
                        contactPrefab.SetActive(true);
                        var position = target.transform.position;
                        var pos = (Vector2) camera.WorldToScreenPoint(position, Camera.MonoOrStereoscopicEye.Mono);
                        contactPrefab.transform.position = new Vector3(pos.x, pos.y, 0);
                        contactText.text = target.transform.name + $"[{Vector3.Distance(position, PlayerDataManager.Instance.WorldHandler.ShipPlayer.transform.position).ToString("F5")}]";
                    }
                    else
                    {
                        contactPrefab.SetActive(false);
                    }
                }
                else
                {
                    contactPrefab.SetActive(false);
                }
            
                foreach (var wsp in spaceObjects)
                {
                    if (camera == null) continue;
                    wsp.Obj.UpdateVisibility();
                    if (wsp.Obj.isVisible && wsp.CanvasPoint)
                    {
                        wsp.CanvasPoint.transform.position = (Vector2) camera.WorldToScreenPoint(wsp.Obj.transform.position, Camera.MonoOrStereoscopicEye.Mono);
                    
                        if (!wsp.isSystem)
                            wsp.CanvasPoint.transform.position = Raycast(wsp.CanvasPoint.transform.position, pointer);
                        wsp.CanvasPoint.SetSelect(wsp.Obj == target);
                        if (PlayerDataManager.Instance.WorldHandler.ShipPlayer.GetTarget() == wsp.Obj)
                        {
                            wsp.CanvasPoint.SetText(wsp.Obj.transform.name + $" [{(Vector3.Distance(wsp.Obj.transform.position, PlayerDataManager.Instance.WorldHandler.ShipPlayer.transform.position)*World.unitSize).ToString("F5")}]");
                        }
                        else
                        {
                            wsp.CanvasPoint.SetText(wsp.Obj.transform.name);
                        }
                    }

                    if (wsp.Obj.isVisible != wsp.CanvasPoint.gameObject.active)
                        wsp.CanvasPoint.gameObject.SetActive(wsp.Obj.isVisible);
                }
            }
            else
            {
                foreach (var wsp in spaceObjects)
                {
                    if (wsp.Obj == null)
                    {
                        Destroy(wsp.CanvasPoint.gameObject);
                        spaceObjects.Remove(wsp);
                        return;
                    }
                }
            }
        }

        private List<RaycastResult> results = new List<RaycastResult>(1);
        public Vector2 Raycast(Vector2 startPos, PointerEventData pointer)
        {
            bool isEnded = false;
            int height = 35;
            int yOffcet = 0;
            while (!isEnded)
            {

                pointer.position = startPos + new Vector2(0, yOffcet);
                results.Clear();
                canvasRaycaster.Raycast(pointer, results);

                if (results.Count == 0)
                {
                    break;
                }

                yOffcet += height;
            }

            return startPos + new Vector2(0, yOffcet);
        }
    }
}
